using Snouter.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Services
{
    public interface ISubcategoryService
    {
        Task<bool> CreateAsync(Subcategory category, CancellationToken token = default);
        Task<Subcategory?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<Subcategory>> GetAllAsync( CancellationToken token = default);

        Task<Subcategory?> UpdateAsync(Subcategory subcategory, CancellationToken token = default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
    }
}
