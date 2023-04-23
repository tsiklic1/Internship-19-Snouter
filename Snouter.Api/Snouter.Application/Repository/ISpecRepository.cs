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
        Task<bool> CreateAsync(Spec spec);
        Task<Spec?> GetByIdAsync(Guid id);
        Task<IEnumerable<Spec>> GetAllAsync();

        Task<bool> UpdateAsync(Spec spec);
        Task<bool> DeleteAsync(Guid id);

        Task<bool> ExistsByIdAsync(Guid id);
    }
}
