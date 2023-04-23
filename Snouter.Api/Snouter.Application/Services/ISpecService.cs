using Snouter.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Services
{
    public interface ISpecService
    {
        Task<bool> CreateAsync(Spec spec);
        Task<Spec?> GetByIdAsync(Guid id);
        Task<IEnumerable<Spec>> GetAllAsync();

        Task<Spec?> UpdateAsync(Spec spec);
        Task<bool> DeleteAsync(Guid id);
    }
}
