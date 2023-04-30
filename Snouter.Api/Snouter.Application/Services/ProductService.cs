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
        public ProductService(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IValidator<Product> productValidator)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productValidator = productValidator;

        }
        public async Task<bool> CreateAsync(Product product, CancellationToken token = default)
        {
            await _productValidator.ValidateAndThrowAsync(product, cancellationToken: token);

            return await _productRepository.CreateAsync(product, token);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _productRepository.DeleteByIdAsync(id, token);
        }

        public Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default)
        {
            return _productRepository.GetAllAsync(token);
        }

        public Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _productRepository.GetByIdAsync(id, token);
        }

        public async Task<Product?> UpdateAsync(Product product, CancellationToken token = default)
        {
            await _productValidator.ValidateAndThrowAsync(product, cancellationToken: token);
            var productExists = await _productRepository.ExistsByIdAsync(product.Id, token);
            if (!productExists)
            {
                return null;
            }

            await _productRepository.UpdateAsync(product, token);
            return product;
        }
    }
}
