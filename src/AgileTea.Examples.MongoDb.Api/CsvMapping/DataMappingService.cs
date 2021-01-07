using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace AgileTea.Examples.MongoDb.Api.CsvMapping
{
    public class DataMappingService : IDataMappingService
    {
        public IDataMap CreateMappings<T>(T[] items) where T : class, IDataMappable, new()
        {
            ICollection<Dictionary<string, string>> mappings = new Collection<Dictionary<string, string>>();

            var headers = CreateHeaders<T>();

            foreach (var item in items)
            {
                var mapping = new Dictionary<string, string>();

                foreach (var header in headers.OrderBy(h => h.Key))
                {

                    var propInfo = item.GetType()
                        .GetProperties()
                        .Single(
                            prop =>
                                ((ColumnHeaderAttribute)
                                    (prop.GetCustomAttributes(typeof(ColumnHeaderAttribute), false)[0])).Name ==
                                header.Value);

                    mapping.Add(header.Value, propInfo.GetValue(item, null).ToString());
                }

                mappings.Add(mapping);
            }

            return new DataMap(mappings, headers);
        }

        public T[] CreateMappedObjects<T>(IDataMap dataMap) where T : class, IDataMappable, new()
        {
            var objects = new Collection<T>();

            foreach (var mapping in dataMap.Mappings)
            {
                var newObject = new T();

                foreach (var header in dataMap.Headers.OrderBy(h => h.Key))
                {
                    var propertyInfo = typeof(T)
                        .GetProperties()
                        .Single(prop => ((ColumnHeaderAttribute)(prop.GetCustomAttributes(typeof(ColumnHeaderAttribute), false)[0])).Name == header.Value);

                    //Convert.ChangeType(mapping[header.Value], propertyInfo.PropertyType)
                    propertyInfo.SetValue(newObject, TypeDescriptor.GetConverter(propertyInfo.PropertyType).ConvertFromInvariantString(mapping[header.Value]), null);
                }

                objects.Add(newObject);
            }

            return objects.ToArray();
        }

        private Dictionary<int, string> CreateHeaders<T>() where T : class, IDataMappable, new()
        {
            var columnHeaderProperties =
                typeof(T)
                    .GetProperties()
                    .Where(prop => prop.IsDefined(typeof(ColumnHeaderAttribute), false));

            return columnHeaderProperties
                .Select(prop => 
                    (ColumnHeaderAttribute)prop.GetCustomAttributes(
                        typeof(ColumnHeaderAttribute)
                        , false)[0])
                        .ToDictionary(
                        columnHeaderAttribute => columnHeaderAttribute.Order, 
                        columnHeaderAttribute => columnHeaderAttribute.Name);
        }
    }
}
