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

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public Task<bool> CreateAsync(Category category)
        {
            return _categoryRepository.CreateAsync(category);
        }

        public Task<Category?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetAllAsync()
        {
            return _categoryRepository.GetAllAsync();
        }

        public Task<Category?> GetByIdAsync(Guid id)
        {
            return _categoryRepository.GetByIdAsync(id);
        }

        //public Task<Category?> UpdateAsync(Category category)
        //{
            
        //}
    }
}
