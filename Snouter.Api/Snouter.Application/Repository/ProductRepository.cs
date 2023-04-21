
using Snouter.Application.Models;

namespace Snouter.Application.Repository
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _products = new List<Product>(); 

        Task<bool> IProductRepository.CreateAsync(Product product)
        {
            if (_products.Contains(product))
            {
                return Task.FromResult(false);
            }

            _products.Add(product);
            return Task.FromResult(true);
        }

        Task<bool> IProductRepository.DeleteAsync(Guid id)
        {
            var tempProduct = _products.SingleOrDefault(x => x.Id == id);
            if (tempProduct is null)
            {
                return Task.FromResult(false);
            }

            _products.Remove(tempProduct);
            return Task.FromResult(true);
        }

        Task<IEnumerable<Product>> IProductRepository.GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Product>>(_products);

        }

        Task<Product?> IProductRepository.GetByIdAsync(Guid id)
        {
            var tempProduct = _products.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(tempProduct);
        }

        Task<bool> IProductRepository.UpdateAsync(Product product)
        {
            var tempProduct = _products.SingleOrDefault(x => x.Id == product.Id);


            if (tempProduct is null)
            {
                return Task.FromResult(false);
            }

            tempProduct.Title= product.Title;
            tempProduct.IsSold= product.IsSold;
            tempProduct.PriceInCents= product.PriceInCents;
            tempProduct.Category = product.Category;
            tempProduct.SubCategory = product.SubCategory;
            tempProduct.Images= product.Images;
            tempProduct.Location = product.Location;
            tempProduct.Properties= product.Properties;

            return Task.FromResult(true);

        }
    }
}
