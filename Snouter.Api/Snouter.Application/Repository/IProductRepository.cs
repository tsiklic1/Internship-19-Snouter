
using Snouter.Application.Models;

namespace Snouter.Application.Repository;
public interface IProductRepository
{
    Task<bool> CreateAsync(Product product);
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>>GetAllAsync();

    //getbyslug 
    Task<bool> UpdateAsync(Product product);
    Task<bool> DeleteAsync(Guid id);

}
