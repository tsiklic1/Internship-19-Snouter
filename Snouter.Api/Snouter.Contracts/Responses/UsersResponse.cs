using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Contracts.Responses
{
    public class UsersResponse
    {
        public IEnumerable<UserResponse> Users { get; set; } = Enumerable.Empty<UserResponse>();

    }
}
