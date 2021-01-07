using System;

namespace AgileTea.Examples.MongoDb.Api.CsvMapping
{
    
    public class ColumnHeaderAttribute : Attribute
    {
        private readonly string name;

        public ColumnHeaderAttribute()
        {
            
        }

        public ColumnHeaderAttribute(string name)
        {
            this.name = name;
        }
        public string Name
        {
            get { return name; }
        }

        public int Order { get; set; }
    }
}
