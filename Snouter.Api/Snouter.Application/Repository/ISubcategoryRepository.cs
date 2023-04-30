using Snouter.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Repository
{
    public interface ISubcategoryRepository
    {
        Task<bool> CreateAsync(Subcategory subcategory, CancellationToken token = default);
        Task<Subcategory?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<Subcategory>> GetAllAsync( CancellationToken token = default);

        Task<bool> UpdateAsync(Subcategory subcategory, CancellationToken token = default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

        Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);

        Task<bool> MatchesCategoryId(Guid subcategoryId, Guid categoryId, CancellationToken token = default);
    }
}
