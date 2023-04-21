using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Contracts.Responses
{
    public class CategoryResponse
    {
        public Guid Id { get; init; }
        public string Title { get; set; }
    }
}
