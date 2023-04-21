using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Contracts.Responses
{
    public class CategoriesResponse
    {
        public IEnumerable<CategoryResponse> Categories { get; set; } = Enumerable.Empty<CategoryResponse>();

    }
}
