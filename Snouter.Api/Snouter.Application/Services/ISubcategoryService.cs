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
        Task<bool> CreateAsync(Subcategory category);
        Task<Subcategory?> GetByIdAsync(Guid id);
        Task<IEnumerable<Subcategory>> GetAllAsync();

        Task<Subcategory?> UpdateAsync(Subcategory subcategory);
        Task<bool> DeleteByIdAsync(Guid id);
    }
}
