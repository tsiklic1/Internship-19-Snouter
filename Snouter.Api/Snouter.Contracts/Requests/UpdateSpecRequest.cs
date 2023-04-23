using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Contracts.Requests
{
    public class UpdateSpecRequest
    {
        public string Title { get; set; }
        public Guid CategoryId { get; set; }
    }
}
