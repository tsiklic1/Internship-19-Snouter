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
        public async Task<bool> CreateAsync(Spec spec, CancellationToken token = default)
        {
            await _specValidator.ValidateAndThrowAsync(spec, cancellationToken: token);
            return await _specRepository.CreateAsync(spec, token);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _specRepository.DeleteByIdAsync(id, token);
        }

        public Task<IEnumerable<Spec>> GetAllAsync(CancellationToken token = default)
        {
            return _specRepository.GetAllAsync(token);
        }

        public Task<Spec?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _specRepository.GetByIdAsync(id, token);
        }

        public async Task<Spec?> UpdateAsync(Spec spec, CancellationToken token = default)
        {
            //var categoryExists = await _categoryRepository.ExistsByIdAsync(spec.CategoryId);
            //if (!categoryExists) { return null; }

            await _specValidator.ValidateAndThrowAsync(spec, cancellationToken: token);

            var specExists = await _specRepository.ExistsByIdAsync(spec.Id, token);
            if (!specExists) { return null; }

            await _specRepository.UpdateAsync(spec, token);
            return spec;
        }
    }
}
