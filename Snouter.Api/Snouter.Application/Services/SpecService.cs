using Snouter.Application.Models;
using Snouter.Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Services
{
    public class SpecService : ISpecService
    {
        private readonly ISpecRepository _specRepository;

        private readonly ICategoryRepository _categoryRepository;

        public SpecService(ISpecRepository specRepository, ICategoryRepository categoryRepository)
        {
            _specRepository = specRepository;
            _categoryRepository = categoryRepository;

        }
        public Task<bool> CreateAsync(Spec spec)
        {
            return _specRepository.CreateAsync(spec);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            return _specRepository.DeleteByIdAsync(id);
        }

        public Task<IEnumerable<Spec>> GetAllAsync()
        {
            return _specRepository.GetAllAsync();
        }

        public Task<Spec?> GetByIdAsync(Guid id)
        {
            return _specRepository.GetByIdAsync(id);
        }

        public async Task<Spec?> UpdateAsync(Spec spec)
        {
            var categoryExists = await _categoryRepository.ExistsByIdAsync(spec.CategoryId);
            if (!categoryExists) { return null; }

            var specExists = await _specRepository.ExistsByIdAsync(spec.Id);
            if (!specExists) { return null; }

            await _specRepository.UpdateAsync(spec);
            return spec;
        }
    }
}
