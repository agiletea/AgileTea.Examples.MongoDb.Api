using System;
using System.IO;
using AgileTea.Examples.MongoDb.Api.Models;

namespace AgileTea.Examples.MongoDb.Api.CsvMapping
{
    internal class FindingsCsvReader : CsvFileReader<FindingCsvModel>, IFindingsCsvReader
    {
        public FindingsCsvReader(Stream stream) : base(stream)
        {
        }

        public FindingsCsvReader(string filename) : base(filename)
        {
            throw new NotSupportedException("Cannot import file through path in server mode. Use a stream and overload method");
        }
    }
}