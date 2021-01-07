using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace AgileTea.Examples.MongoDb.Api.Entities
{
    public class Anomaly
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
    }
}
