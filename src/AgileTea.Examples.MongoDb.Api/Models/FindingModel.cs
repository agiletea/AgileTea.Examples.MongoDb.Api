using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgileTea.Examples.MongoDb.Api.CsvMapping;

namespace AgileTea.Examples.MongoDb.Api.Models
{
    public class FindingModel : IDataMappable
    {
        public string? Id { get; set; }

        public long Timestamp { get; set; }
        
        public DateTime? Created { get; set; }

        public DateTime? LastUpdated { get; set; }
        
        public Guid GlobalId { get; set; }

        public string Title { get; set; } = default!;

        public int Severity { get; set; }

        public AnomalyModel? Anomaly { get; set; }

        public InspectorModel Inspector { get; set; } = default!;
    }
}
