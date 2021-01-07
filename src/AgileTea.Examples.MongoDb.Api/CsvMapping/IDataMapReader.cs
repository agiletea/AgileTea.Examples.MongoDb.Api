using System;

namespace AgileTea.Examples.MongoDb.Api.CsvMapping
{
    public interface IDataMapReader : IDisposable
    {
        IDataMap ReadMappings();
    }
}
