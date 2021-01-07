using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace AgileTea.Examples.MongoDb.Api.Entities
{
    public class Inspector
    {
        [BsonId]
        public int Id { get; set; }

        public string Name { get; set; } = default!;
    }
}
