using FluentValidation;
using Snouter.Application.Models;
using Snouter.Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Validators
{
    public class SubcategoryValidator : AbstractValidator<Subcategory>
    {
        private readonly ICategoryRepository _categoryRepository;
        public SubcategoryValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository= categoryRepository;

            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Title).NotEmpty();  

            RuleFor(x => x.CategoryId).NotEmpty();

            RuleFor(x => x.CategoryId)
                .MustAsync(ValidateCategory)
                .WithMessage("Category does not exist!");
        }

        private async Task<bool> ValidateCategory(Subcategory subcategory, Guid categoryId, CancellationToken token)
        {
            var categoryExists = await _categoryRepository.ExistsByIdAsync(categoryId);
            return categoryExists;
        }
    }
}
