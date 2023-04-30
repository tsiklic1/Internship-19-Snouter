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
        public async Task<bool> CreateAsync(Subcategory subcategory, CancellationToken token = default)
        {
            await _subcategoryValidator.ValidateAndThrowAsync(subcategory, cancellationToken: token);
            return await _subcategoryRepository.CreateAsync(subcategory, token);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _subcategoryRepository.DeleteByIdAsync(id, token);
        }

        public Task<IEnumerable<Subcategory>> GetAllAsync(CancellationToken token = default)
        {
            return _subcategoryRepository.GetAllAsync(token);
        }

        public Task<Subcategory?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _subcategoryRepository.GetByIdAsync(id, token);
        }

        public async Task<Subcategory?> UpdateAsync(Subcategory subcategory, CancellationToken token = default)
        {
            await _subcategoryValidator.ValidateAndThrowAsync(subcategory, cancellationToken: token);
            var subcategoryExists = await _subcategoryRepository.ExistsByIdAsync(subcategory.Id, token);
            if (!subcategoryExists) { return null; }


            await _subcategoryRepository.UpdateAsync(subcategory, token);
            return subcategory;
        }
    }
}
