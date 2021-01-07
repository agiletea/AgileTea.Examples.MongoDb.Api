using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileTea.Examples.MongoDb.Api.Models
{
    public class AnomalyModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
    }
}
