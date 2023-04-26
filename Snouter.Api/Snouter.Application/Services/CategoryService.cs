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
        public async Task<bool> CreateAsync(Category category)
        {
            await _categoryValidator.ValidateAndThrowAsync(category);
            return await _categoryRepository.CreateAsync(category);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            return _categoryRepository.DeleteByIdAsync(id);
        }

        public Task<IEnumerable<Category>> GetAllAsync()
        {
            return _categoryRepository.GetAllAsync();
        }

        public Task<Category?> GetByIdAsync(Guid id)
        {
            return _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            await _categoryValidator.ValidateAndThrowAsync(category);
            var categoryExists = await _categoryRepository.ExistsByIdAsync(category.Id);
            if (!categoryExists) { return null; }
            await _categoryRepository.UpdateAsync(category);
            return category;
        }
    }
}
