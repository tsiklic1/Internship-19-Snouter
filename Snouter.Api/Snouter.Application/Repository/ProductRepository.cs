
using Snouter.Application.Models;

namespace Snouter.Application.Repository
{
    public class ProductRepository : IProductRepository
    {
        Task<bool> IProductRepository.CreateAsync(Product product)
        {
            throw new NotImplementedException();
        }

        Task<bool> IProductRepository.DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Product>> IProductRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<Product?> IProductRepository.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<bool> IProductRepository.UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
