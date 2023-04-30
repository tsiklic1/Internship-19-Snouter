using Snouter.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Repository
{
    public interface ICategoryRepository
    {
        Task<bool> CreateAsync(Category category, CancellationToken token = default);
        Task<Category?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<Category>> GetAllAsync( CancellationToken token = default);

        Task<bool> UpdateAsync(Category category, CancellationToken token = default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

        Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
    }
}
