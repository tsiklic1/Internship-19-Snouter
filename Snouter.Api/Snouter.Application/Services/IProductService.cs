using Snouter.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Services
{
    public interface IProductService
    {
        Task<bool> CreateAsync(Product product);
        Task<Product?> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product?> UpdateAsync(Product product);
        Task<Product?> DeleteAsync(Guid id);
    }
}
