using Snouter.Application.Models;
using Snouter.Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;

        }
        public Task<bool> CreateAsync(Product product)
        {
            return _productRepository.CreateAsync(product);
        }

        public Task<Product?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return _productRepository.GetAllAsync();
        }

        public Task<Product?> GetByIdAsync(Guid id)
        {
            return _productRepository.GetByIdAsync(id);
        }

        public Task<Product?> UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
