using Snouter.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Services
{
    public interface IUserService
    {
        Task<bool> CreateAsync(User user, CancellationToken token = default);
        Task<User?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<User>> GetAllAsync(CancellationToken token = default);

        Task<User?> UpdateAsync(User user, CancellationToken token = default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
    }
}
