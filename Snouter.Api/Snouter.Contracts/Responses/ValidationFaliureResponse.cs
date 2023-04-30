using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Contracts.Responses
{
    public class ValidationFaliureResponse
    {
        public IEnumerable<ValidationResponse> Errors { get; set; }
    }

    public class ValidationResponse
    {
        public string PropertyName { get; init; }
        public string  Message { get; init; }    
    }
}
