using Snouter.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Repository
{
    public interface ISpecRepository
    {
        Task<bool> CreateAsync(Spec spec, CancellationToken token = default);
        Task<Spec?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<Spec>> GetAllAsync(CancellationToken token = default);

        Task<bool> UpdateAsync(Spec spec, CancellationToken token = default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

        Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
    }
}
