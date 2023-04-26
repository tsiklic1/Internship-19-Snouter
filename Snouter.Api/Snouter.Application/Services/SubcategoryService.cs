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
    public class SubcategoryService : ISubcategoryService
    {
        private readonly ISubcategoryRepository _subcategoryRepository;

        private readonly ICategoryRepository _categoryRepository;

        private readonly IValidator<Subcategory> _subcategoryValidator;

        public SubcategoryService(ISubcategoryRepository subcategoryRepository,
            ICategoryRepository categoryRepository,
            IValidator<Subcategory> subcategoryValidator)
        {
            _subcategoryRepository = subcategoryRepository;
            _categoryRepository = categoryRepository;
            _subcategoryValidator = subcategoryValidator;

        }
        public async Task<bool> CreateAsync(Subcategory subcategory)
        {
            await _subcategoryValidator.ValidateAndThrowAsync(subcategory);
            return await _subcategoryRepository.CreateAsync(subcategory);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            return _subcategoryRepository.DeleteByIdAsync(id);
        }

        public Task<IEnumerable<Subcategory>> GetAllAsync()
        {
            return _subcategoryRepository.GetAllAsync();
        }

        public Task<Subcategory?> GetByIdAsync(Guid id)
        {
            return _subcategoryRepository.GetByIdAsync(id);
        }

        public async Task<Subcategory?> UpdateAsync(Subcategory subcategory)
        {
            await _subcategoryValidator.ValidateAndThrowAsync(subcategory);
            var subcategoryExists = await _subcategoryRepository.ExistsByIdAsync(subcategory.Id);
            if (!subcategoryExists) { return null; }


            await _subcategoryRepository.UpdateAsync(subcategory);
            return subcategory;
        }
    }
}
