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
        Task<bool> CreateAsync(Product product, CancellationToken token = default);
        Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default);

        Task<Product?> UpdateAsync(Product product, CancellationToken token = default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
    }
}
