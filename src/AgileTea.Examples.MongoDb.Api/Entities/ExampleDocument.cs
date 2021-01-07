using AgileTea.Persistence.Common.Entities;

namespace AgileTea.Examples.MongoDb.Api.Entities
{
    public class ExampleDocument : GuidIndexedEntityBase
    {
        public string Name { get; set; } = default!;
        
        public ExampleNestedChildDocument? Child { get; set; }
    }
}
