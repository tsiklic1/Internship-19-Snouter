using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Models
{
    public class Subcategory
    {
        public Guid Id { get; init; }

        public string Title { get; set; }

        public Guid CategoryId { get; init; }

        public Category Category { get; set; }
    }
}
