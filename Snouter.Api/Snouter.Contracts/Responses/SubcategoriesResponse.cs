using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Contracts.Responses
{
    public class SubcategoriesResponse
    {
        public IEnumerable<SubcategoryResponse> Subcategories { get; set; } = Enumerable.Empty<SubcategoryResponse>();

    }
}
