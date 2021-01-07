using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgileTea.Examples.MongoDb.Api.Entities;
using AgileTea.Examples.MongoDb.Api.Models;
using MongoDB.Bson;

namespace AgileTea.Examples.MongoDb.Api.Adapters
{
    public class FindingDocumentAdapter
    {
        public FindingModel ToApiModel(FindingDocument document)
        {
            return new FindingModel
            {
                LastUpdated = document.LastUpdated,
                GlobalId = document.GlobalId,
                Id = document.Id.ToString(),
                Created = document.Created,
                Title = document.Title,
                Timestamp = document.Timestamp == null ? 0 : document.Timestamp.Value,
                Anomaly = document.Anomaly == null ? null : ToApiModel(document.Anomaly),
                Inspector = ToApiModel(document.Inspector)
            };
        }

        public AnomalyModel ToApiModel(Anomaly document)
        {
            return new AnomalyModel
            {
                Id = document.Id,
                Title = document.Title
            };
        }

        public InspectorModel ToApiModel(Inspector document)
        {
            return new InspectorModel
            {
                Id = document.Id,
                Name = document.Name
            };
        }

        public FindingDocument ToDocument(FindingModel model)
        {
            var document = new FindingDocument();

            if (!string.IsNullOrEmpty(model.Id))
            {
                document.Id = new ObjectId(model.Id);
            }

            document.GlobalId = model.GlobalId;
            
            document.Title = model.Title;

            document.Severity = model.Severity;
            
            document.Timestamp = document.SetTimestamp();

            if (model.Anomaly != null)
            {
                document.Anomaly = ToDocument(model.Anomaly);
            }

            document.Inspector = ToDocument(model.Inspector);

            return document;
        }

        public Anomaly ToDocument(AnomalyModel model) => new Anomaly {Title = model.Title, Id = model.Id};

        public Inspector ToDocument(InspectorModel model) => new Inspector { Name = model.Name, Id = model.Id };

        public FindingDocument ToDocument(FindingCsvModel model)
        {
            var document = new FindingDocument();

            if (!string.IsNullOrEmpty(model.Id))
            {
                document.Id = new ObjectId(model.Id);
            }

            document.GlobalId = model.GlobalId;

            document.Title = model.Title;

            document.Severity = model.Severity;

            document.Timestamp = document.SetTimestamp();

            if (model.AnomalyId != Guid.Empty)
            {
                document.Anomaly = new Anomaly
                {
                    Id = model.AnomalyId,
                    Title = model.AnomalyTitle
                };
            }

            document.Inspector = new Inspector
            {
                Id = model.InspectorId,
                Name = model.InspectorName
            };

            return document;
        }
    }
}
