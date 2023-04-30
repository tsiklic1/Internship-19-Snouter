
using Snouter.Application.Models;

namespace Snouter.Application.Repository;
public interface IProductRepository
{
    Task<bool> CreateAsync(Product product, CancellationToken token = default);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default);
    Task<IEnumerable<Product>>GetAllAsync( CancellationToken token = default);

    Task<bool> UpdateAsync(Product product, CancellationToken token = default);
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);

    Task<bool> SpecsMachCategory(Product product, CancellationToken token = default);

}
