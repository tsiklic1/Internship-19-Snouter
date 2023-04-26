using FluentValidation;
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

        private readonly IValidator<Product> _productValidator;




        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IValidator<Product> productValidator)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productValidator = productValidator;

        }
        public async Task<bool> CreateAsync(Product product)
        {
            await _productValidator.ValidateAndThrowAsync(product);

            return await _productRepository.CreateAsync(product);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            return _productRepository.DeleteByIdAsync(id);
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return _productRepository.GetAllAsync();
        }

        public Task<Product?> GetByIdAsync(Guid id)
        {
            return _productRepository.GetByIdAsync(id);
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var productExists = await _productRepository.ExistsByIdAsync(product.Id);
            if (!productExists)
            {
                return null;
            }

            await _productRepository.UpdateAsync(product);
            return product;
        }
    }
}
