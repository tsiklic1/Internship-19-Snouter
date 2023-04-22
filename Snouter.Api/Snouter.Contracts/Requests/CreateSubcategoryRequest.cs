using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Contracts.Requests
{
    public class CreateSubcategoryRequest
    {
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
    }
}
