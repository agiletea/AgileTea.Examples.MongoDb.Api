using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace AgileTea.Examples.MongoDb.Api.CsvMapping
{
    public class CsvFileReader<T> : StreamReader, IDataMapReader where T : class, IDataMappable, new()
    {
        public CsvFileReader(Stream stream) : base(stream)
        {
        }

        public CsvFileReader(string filename)
            : base(filename)
        {
        }

        public IDataMap ReadMappings()
        {
            // get info about mapable object
            var columnHeaderProperties =
               typeof(T)
               .GetProperties()
               .Where(prop => prop.IsDefined(typeof(ColumnHeaderAttribute), false))
               .ToArray();

            var headers =
                columnHeaderProperties.Select(
                    prop => (ColumnHeaderAttribute)prop.GetCustomAttributes(typeof(ColumnHeaderAttribute), false)[0])
                                      .ToDictionary(
                                          columnHeaderAttribute => columnHeaderAttribute.Order,
                                          columnHeaderAttribute => columnHeaderAttribute.Name);

            // check if first row is a header row
            var firstRow = ReadLine();

            if (String.IsNullOrEmpty(firstRow))
            {
                // todo - need to provide some logging here and non exception handling
                throw new InvalidOperationException("Unexpected csv file format with file - empty or null data");
            }

            var values = ExtractCsvRowValues(firstRow);

            if (values.Length == 0)
            {
                // todo - need to provide some logging here and non exception handling
                throw new InvalidOperationException("Unexpected csv file format with file");
            }
            if (values.Length != headers.Count)
            {
                // todo - need to provide some logging here and non exception handling
                throw new InvalidOperationException(
                    "Unexpected csv file format with file (line items length does not match expected mappable item property count)");
            }

            var index = 0;
            var isHeaderRow = true;

            var firstMapping = new Dictionary<string, string>();

            foreach (var header in headers.OrderBy(h => h.Key))
            {
                if (!values[index].Equals(header.Value, StringComparison.CurrentCultureIgnoreCase))
                {
                    isHeaderRow = false;
                }

                firstMapping.Add(header.Value, values[index]);

                index++;
            }

            ICollection<Dictionary<string, string>> mappedCollection = new Collection<Dictionary<string, string>>();

            // first row was not header, so add data
            if (!isHeaderRow)
            {
                mappedCollection.Add(firstMapping);
            }
            
            while (!EndOfStream)
            {
                var currentRow = ReadLine();

                if (!String.IsNullOrEmpty(currentRow))
                {
                    values = ExtractCsvRowValues(currentRow);
                    var mappings = new Dictionary<string, string>();

                    index = 0;
                    foreach (var header in headers.OrderBy(h => h.Key))
                    {
                        mappings.Add(header.Value, values[index]);
                        index++;
                    }

                    mappedCollection.Add(mappings);
                }
            }

            return new DataMap(mappedCollection, headers);
        }

        private string[] ExtractCsvRowValues(string csvRow)
        {
            var rawValues = csvRow.Split(',');

            return rawValues.Select(val => val.Replace("\"", String.Empty)).ToArray();
        }
    }
}
