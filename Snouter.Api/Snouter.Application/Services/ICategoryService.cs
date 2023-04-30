using Snouter.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Services
{
    public interface ICategoryService
    {
        Task<bool> CreateAsync(Category category, CancellationToken token = default);
        Task<Category?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<Category>> GetAllAsync( CancellationToken token = default);

        Task<Category?> UpdateAsync(Category category, CancellationToken token = default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

    }
}
