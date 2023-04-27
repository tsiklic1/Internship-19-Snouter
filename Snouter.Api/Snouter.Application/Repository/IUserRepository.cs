using Snouter.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Repository
{
    public interface IUserRepository
    {
        Task<bool> CreateAsync(User user, CancellationToken token = default);
        Task<User?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<User>> GetAllAsync(CancellationToken token = default);

        Task<bool> UpdateAsync(User user, CancellationToken token = default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

        Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
    }
}
