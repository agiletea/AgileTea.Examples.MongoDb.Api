using System;

namespace AgileTea.Examples.MongoDb.Api.Models
{
    public class NestedChildModel
    {
        public Guid Id { get; set; }

        public string ChildName { get; set; } = default!;
    }
}
