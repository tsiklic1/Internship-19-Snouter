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
    public class SpecService : ISpecService
    {
        private readonly ISpecRepository _specRepository;

        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<Spec> _specValidator;

        public SpecService(ISpecRepository specRepository,
            ICategoryRepository categoryRepository,
            IValidator<Spec> specValidator)
        {
            _specRepository = specRepository;
            _categoryRepository = categoryRepository;
            _specValidator = specValidator;

        }
        public async Task<bool> CreateAsync(Spec spec)
        {
            await _specValidator.ValidateAndThrowAsync(spec);
            return await _specRepository.CreateAsync(spec);
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
            //var categoryExists = await _categoryRepository.ExistsByIdAsync(spec.CategoryId);
            //if (!categoryExists) { return null; }

            await _specValidator.ValidateAndThrowAsync(spec);

            var specExists = await _specRepository.ExistsByIdAsync(spec.Id);
            if (!specExists) { return null; }

            await _specRepository.UpdateAsync(spec);
            return spec;
        }
    }
}
