using System.Collections.Generic;

namespace AgileTea.Examples.MongoDb.Api.CsvMapping
{
    public interface IDataMap
    {
        ICollection<Dictionary<string, string>> Mappings { get; }

        Dictionary<int, string> Headers { get; }
    }
}
