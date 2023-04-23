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

        public SubcategoryService(ISubcategoryRepository subcategoryRepository, ICategoryRepository categoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
            _categoryRepository = categoryRepository;

        }
        public Task<bool> CreateAsync(Subcategory subcategory)
        {
            //var categoryExists = await _categoryRepository.ExistsByIdAsync(subcategory.Id);
            //if (!categoryExists)
            //{
            //    return false;
            //}

            return _subcategoryRepository.CreateAsync(subcategory);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            return _subcategoryRepository.DeleteAsync(id);
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
            var categoryExists = await _categoryRepository.ExistsByIdAsync(subcategory.CategoryId);
            if (!categoryExists) { return null; }

            var subcategoryExists = await _subcategoryRepository.ExistsByIdAsync(subcategory.Id);
            if (!subcategoryExists) { return null; }

            await _subcategoryRepository.UpdateAsync(subcategory);
            return subcategory;
        }
    }
}
