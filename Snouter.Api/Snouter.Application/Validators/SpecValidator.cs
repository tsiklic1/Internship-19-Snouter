using FluentValidation;
using Snouter.Application.Models;
using Snouter.Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Snouter.Application.Validators
{
    public class SpecValidator : AbstractValidator<Spec>
    {
        private readonly ICategoryRepository _categoryRepository;
        public SpecValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Title).NotEmpty();

            RuleFor(x => x.CategoryId).NotEmpty();

            RuleFor(x => x.CategoryId)
                .MustAsync(ValidateCategory)
                .WithMessage("Category does not exist!");
        }

        private async Task<bool> ValidateCategory(Spec spec, Guid categoryId, CancellationToken token)
        {
            var categoryExists = await _categoryRepository.ExistsByIdAsync(categoryId);
            return categoryExists;
        }
    }
}
