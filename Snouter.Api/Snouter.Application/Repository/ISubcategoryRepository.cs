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
        Task<bool> CreateAsync(Subcategory subcategory);
        Task<Subcategory?> GetByIdAsync(Guid id);
        Task<IEnumerable<Subcategory>> GetAllAsync();

        Task<bool> UpdateAsync(Subcategory subcategory);
        Task<bool> DeleteAsync(Guid id);

        Task<bool> ExistsByIdAsync(Guid id);
    }
}
