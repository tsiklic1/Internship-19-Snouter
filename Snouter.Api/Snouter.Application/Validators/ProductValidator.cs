using FluentValidation;
using Snouter.Application.Models;
using Snouter.Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Snouter.Application.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubcategoryRepository _subcategoryRepository;
        private readonly IUserRepository _userRepository;
        public ProductValidator(ICategoryRepository categoryRepository,
            ISubcategoryRepository subcategoryRepository, IUserRepository userRepository)
        {
            _categoryRepository = categoryRepository;
            _subcategoryRepository = subcategoryRepository;
            _userRepository = userRepository;

            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.IsSold).NotEmpty();
            RuleFor(x => x.PriceInCents).NotEmpty();
            RuleFor(x => x.Location).NotEmpty();
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.SubcategoryId).NotEmpty();
            RuleFor(x => x.SellerId).NotEmpty();

            RuleFor(x => x.PriceInCents)
                .Must(ValidatePrice)
                .WithMessage("Price has to be a positive number.");

            RuleFor(x => x.CategoryId)
                .MustAsync(ValidateCategory)
                .WithMessage("Category does not exist");

            RuleFor(x => x.SubcategoryId)
                .MustAsync(ValidateSubcategory)
                .WithMessage("Subcategory does not exist");

            RuleFor(x => x.SellerId)
                .MustAsync(ValidateSeller)
                .WithMessage("Seller does not exist");

            //rule da subkategorija pripada kategoriji
            //rule da svi specovi pripadaju kategoriji

            RuleFor(x => x)
                .MustAsync(ValidateSubcategoryMatchesCategory)
                .WithMessage("Inserted subcategory does not belong to category");

        }
        private bool ValidatePrice(int priceInCents)
        {
            return priceInCents > 0;
        }

        private async Task<bool> ValidateCategory(Guid categoryId, CancellationToken token)
        {
            var categoryExists = await _categoryRepository.ExistsByIdAsync(categoryId);
            return categoryExists;
        }

        private async Task<bool> ValidateSubcategory(Guid subcategoryId, CancellationToken token)
        {
            var subcategoryExists = await _subcategoryRepository.ExistsByIdAsync(subcategoryId);
            return subcategoryExists;
        }

        private async Task<bool> ValidateSeller(Guid sellerId, CancellationToken token)
        {
            var sellerExists = await _userRepository.ExistsByIdAsync(sellerId);
            return sellerExists;
        }

        private async Task<bool> ValidateSubcategoryMatchesCategory(Product product, CancellationToken token)
        {
            return await _subcategoryRepository.MatchesCategoryId(product.SubcategoryId, product.CategoryId);
        }
    }
}
