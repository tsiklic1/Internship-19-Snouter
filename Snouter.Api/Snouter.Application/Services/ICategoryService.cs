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
        Task<bool> CreateAsync(Category category);
        Task<Category?> GetByIdAsync(Guid id);
        Task<IEnumerable<Category>> GetAllAsync();

        //Task<Category?> UpdateAsync(Category category);
        Task<Category?> DeleteAsync(Guid id);

    }
}
