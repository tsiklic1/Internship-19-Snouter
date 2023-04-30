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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        private readonly IValidator<Category> _categoryValidator;

        public CategoryService(ICategoryRepository categoryRepository, IValidator<Category> categoryValidator)
        {
            _categoryRepository = categoryRepository;
            _categoryValidator = categoryValidator;
        }
        public async Task<bool> CreateAsync(Category category, CancellationToken token = default)
        {
            await _categoryValidator.ValidateAndThrowAsync(category,cancellationToken: token);
            return await _categoryRepository.CreateAsync(category, token);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _categoryRepository.DeleteByIdAsync(id, token);
        }

        public Task<IEnumerable<Category>> GetAllAsync(CancellationToken token = default)
        {
            return _categoryRepository.GetAllAsync(token);
        }

        public Task<Category?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _categoryRepository.GetByIdAsync(id, token);
        }

        public async Task<Category?> UpdateAsync(Category category, CancellationToken token = default)
        {
            await _categoryValidator.ValidateAndThrowAsync(category, cancellationToken: token);
            var categoryExists = await _categoryRepository.ExistsByIdAsync(category.Id, token);
            if (!categoryExists) { return null; }
            await _categoryRepository.UpdateAsync(category, token);
            return category;
        }
    }
}
