using Microsoft.Extensions.Hosting;
using Snouter.Api.Controllers;
using Snouter.Application.Models;
using Snouter.Contracts.Requests;
using Snouter.Contracts.Responses;

namespace Snouter.Api.Mapping
{
    public static class ContractMapping
    {
        public static Product MapToProduct(this CreateProductRequest request)
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                IsSold = request.IsSold,
                PriceInCents = request.PriceInCents,
                Category = request.Category,
                SubCategory = request.SubCategory,
                Images = request.Images,
                Properties = request.Properties,

            };
        }

        public static Product MapToProduct(this UpdateProductRequest request, Guid id)
        {
            return new Product
            {
                Id = id,
                Title = request.Title,
                IsSold = request.IsSold,
                PriceInCents = request.PriceInCents,
                Category = request.Category,
                SubCategory = request.SubCategory,
                Images = request.Images,
                Properties = request.Properties,
            };
        }

        public static ProductResponse MapToResponse(this Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Title = product.Title,
                IsSold = product.IsSold,
                PriceInCents = product.PriceInCents,
                Category = product.Category,
                SubCategory = product.SubCategory,
                Images = product.Images,
                Properties = product.Properties,
            };
        }

        public static ProductsResponse MapToResponse(this IEnumerable<Product> products)
        {
            return new ProductsResponse
            {
                Products = products.Select(MapToResponse)
            };
        }

        public static Category MapToCategory(this CreateCategoryRequest request) {
            return new Category
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
            };
        }

        public static Category MapToCategory(this UpdateCategoryRequest request, Guid id)
        {
            return new Category
            {
                Id = id,
                Title = request.Title,
            };
        }

        public static CategoryResponse MapToResponse(this Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                Title = category.Title
            };
        }

        public static CategoriesResponse MapToResponse(this IEnumerable<Category> categories)
        {
            return new CategoriesResponse
            {
                Categories = categories.Select(MapToResponse)
            };
        }

        public static Subcategory MapToSubcategory(this CreateSubcategoryRequest request)
        {
            return new Subcategory
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Category = new Category{
                    Id = request.CategoryId,
                    //Title = request.Title,
                }
            };
        }

        //update mapping not implemented

        public static SubcategoryResponse MapToResponse(this Subcategory subcategory)
        {
            return new SubcategoryResponse
            {
                Id = subcategory.Id,
                Title = subcategory.Title,
                CategoryId = subcategory.Category.Id
            };
        }

        public static SubcategoriesResponse MapToResponse(this IEnumerable<Subcategory> subcategories)
        {
            return new SubcategoriesResponse
            {
                Subcategories = subcategories.Select(MapToResponse)
            };
        }
    }
}
