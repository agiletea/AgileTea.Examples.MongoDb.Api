using System;

namespace AgileTea.Examples.MongoDb.Api.Models
{
    public class ExampleDocumentModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;
        
        public NestedChildModel? Child { get; set; }
    }
}
