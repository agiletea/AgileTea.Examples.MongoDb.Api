using AgileTea.Persistence.Common.Entities;

namespace AgileTea.Examples.MongoDb.Api.Entities
{
    public class ExampleNestedChildDocument : GuidIndexedEntityBase
    {
        public string ChildName { get; set; } = default!;
    }
}
