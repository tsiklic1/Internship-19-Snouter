using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Contracts.Responses
{
    public class SpecsResponse
    {
        public IEnumerable<SpecResponse> Specs { get; set; } = Enumerable.Empty<SpecResponse>();
    }
}
