using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgileTea.Examples.MongoDb.Api.CsvMapping;

namespace AgileTea.Examples.MongoDb.Api.Models
{
    public class FindingCsvModel : IDataMappable
    {
        [ColumnHeader("_id", Order = 0)]
        public string? Id { get; set; }

        [ColumnHeader("globalId", Order = 1)]
        public Guid GlobalId { get; set; }

        [ColumnHeader("ts_str", Order = 2)]
        public long TimestampAsString { get; set; }

        [ColumnHeader("anomaly._id", Order = 3)]
        public Guid AnomalyId { get; set; }

        [ColumnHeader("anomaly.title", Order = 4)]
        public string AnomalyTitle { get; set; } = default!;

        [ColumnHeader("anomaly.idBase", Order = 5)]
        public string AnomalyIdBase { get; set; } = default!;

        [ColumnHeader("title", Order = 6)]
        public string Title { get; set; } = default!;

        [ColumnHeader("severity", Order = 7)]
        public int Severity { get; set; }

        [ColumnHeader("inspector._id", Order = 8)]
        public int InspectorId { get; set; }

        [ColumnHeader("inspector.name", Order = 9)]
        public string InspectorName { get; set; }
    }
}
