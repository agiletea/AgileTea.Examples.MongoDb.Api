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
    public class ExampleDocumentAdapter
    {
        public ExampleDocumentModel ToApiModel(ExampleDocument? document)
        {
            return document == null ? null : new ExampleDocumentModel
            {
                Id = document.Id,
                Name = document.Name,
                Child = document.Child == null ? null : ToApiModel(document.Child)
            };
        }

        public ExampleDocument ToDocument(ExampleDocumentModel model)
        {
            var document = new ExampleDocument();

            document.Id = model.Id;
            
            document.Name = model.Name;

            if (model.Child != null)
            {
                document.Child = ToDocument(model.Child);
            }

            return document;
        }

        public NestedChildModel ToApiModel(ExampleNestedChildDocument document)
        {
            return new NestedChildModel
            {
                Id = document.Id,
                ChildName = document.ChildName
            };
        }

        public ExampleNestedChildDocument ToDocument(NestedChildModel model)
        {
            var document = new ExampleNestedChildDocument();

            document.Id = model.Id;
            
            document.ChildName = model.ChildName;

            return document;
        }

    }
}
