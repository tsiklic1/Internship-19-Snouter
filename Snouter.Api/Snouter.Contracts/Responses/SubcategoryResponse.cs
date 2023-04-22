using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Contracts.Responses
{
    public class SubcategoryResponse
    {
        public Guid Id { get; init; }
        public string Title { get; set; }
        public Guid CategoryId { get; set; }
    }
}
