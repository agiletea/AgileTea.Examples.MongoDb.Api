using System.Collections.Generic;

namespace AgileTea.Examples.MongoDb.Api.CsvMapping
{
    public class DataMap : IDataMap
    {
        private readonly ICollection<Dictionary<string, string>> mappings;

        private readonly Dictionary<int, string> headers;

        public DataMap(
            ICollection<Dictionary<string, string>> mappings, 
            Dictionary<int, string> headers)
        {
            this.mappings = mappings;
            this.headers = headers;
        }

        public ICollection<Dictionary<string, string>> Mappings
        {
            get
            {
                return mappings;
            }
        }

        public Dictionary<int, string> Headers
        {
            get
            {
                return headers;
            }
        }
    }
}
