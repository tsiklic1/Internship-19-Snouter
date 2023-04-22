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

        private readonly CategoryRepository _categoryRepository;

        public SubcategoryService(ISubcategoryRepository subcategoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;

        }
        public Task<bool> CreateAsync(Subcategory subcategory)
        {
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
    }
}
