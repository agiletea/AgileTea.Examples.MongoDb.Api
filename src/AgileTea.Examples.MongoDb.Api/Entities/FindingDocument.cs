using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgileTea.Persistence.Mongo.Entities;

namespace AgileTea.Examples.MongoDb.Api.Entities
{
    public class FindingDocument : TimestampedIndexedEntityBase
    {
        public Guid GlobalId { get; set; }

        public string Title { get; set; } = default!;
        
        public int Severity { get; set; }
        
        public Anomaly? Anomaly { get; set; }

        public Inspector Inspector { get; set; } = default!;
    }
}
