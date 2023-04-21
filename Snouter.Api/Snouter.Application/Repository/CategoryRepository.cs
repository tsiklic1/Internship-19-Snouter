using Snouter.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private List<Category> _categories = new List<Category>();
        public Task<bool> CreateAsync(Category category)
        {
            if (_categories.Contains(category))
            {
                return Task.FromResult(false);
            }
            _categories.Add(category);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            var tempCategory = _categories.SingleOrDefault(c => c.Id == id);
            if (tempCategory is null) {
                return Task.FromResult(false);
            }
            _categories.Remove(tempCategory);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Category>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Category>>(_categories);
        }


        public Task<Category?> GetByIdAsync(Guid id)
        {
            var tempCategory = _categories.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(tempCategory);
        }

        public Task<bool> UpdateAsync(Category category)
        {
            var tempCategory = _categories.SingleOrDefault(x => x.Id == category.Id);


            if (tempCategory is null)
            {
                return Task.FromResult(false);
            }

            tempCategory.Title = category.Title;

            return Task.FromResult(true);
        }
    }
}
