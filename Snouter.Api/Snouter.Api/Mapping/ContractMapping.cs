using Microsoft.Extensions.Hosting;
using Snouter.Api.Controllers;
using Snouter.Application.Models;
using Snouter.Contracts.Requests;
using Snouter.Contracts.Responses;
using System.Net.NetworkInformation;

namespace Snouter.Api.Mapping
{
    public static class ContractMapping
    {
        public static Product MapToProduct(this CreateProductRequest request)
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Title= request.Title,
                IsSold= request.IsSold,
                PriceInCents= request.PriceInCents,
                CategoryId= request.CategoryId,
                SubcategoryId= request.SubcategoryId,
                SellerId= request.SellerId,
                Images = request.Images,
                Specs = request.Specs,
                Location= request.Location,
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
                CategoryId = request.CategoryId,
                SubcategoryId = request.SubcategoryId,
                SellerId = request.SellerId,
                Images = request.Images,
                Specs = request.Specs,
                Location = request.Location,
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
                CategoryId = product.CategoryId,
                SubcategoryId = product.SubcategoryId,
                SellerId= product.SellerId,
                Images = product.Images,
                Specs = product.Specs,
                Location = product.Location,
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
                CategoryId = request.CategoryId
            };
        }

        public static Subcategory MapToSubcategory(this UpdateSubcategoryRequest request, Guid id)
        {
            return new Subcategory
            {
                Id = id,
                Title = request.Title,
                CategoryId = request.CategoryId
            };
        }
        public static SubcategoryResponse MapToResponse(this Subcategory subcategory)
        {
            return new SubcategoryResponse
            {
                Id = subcategory.Id,
                Title = subcategory.Title,
                CategoryId = subcategory.CategoryId
            };
        }

        public static SubcategoriesResponse MapToResponse(this IEnumerable<Subcategory> subcategories)
        {
            return new SubcategoriesResponse
            {
                Subcategories = subcategories.Select(MapToResponse)
            };
        }

        public static User MapToUser(this CreateUserRequest request) 
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Password = request.Password,
                IsAdmin = request.IsAdmin
            };
        }

        public static User MapToUser(this UpdateUserRequest request, Guid id)
        {
            return new User
            {
                Id = id,
                Name = request.Name,
                Password = request.Password,
                IsAdmin = request.IsAdmin
            };
        }

        public static UserResponse MapToResponse(this User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Password = user.Password,
                IsAdmin = user.IsAdmin
            };
        }

        public static UsersResponse MapToResponse(this IEnumerable<User> users)
        {
            return new UsersResponse
            {
                Users = users.Select(MapToResponse)
            };
        }
        public static Spec MapToSpec(this CreateSpecRequest request)
        {
            return new Spec
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                CategoryId = request.CategoryId
            };
        }

        public static Spec MapToSpec(this UpdateSpecRequest request, Guid id)
        {
            return new Spec
            {
                Id = id,
                Title = request.Title,
                CategoryId = request.CategoryId
            };
        }

        public static SpecResponse MapToResponse(this Spec spec)
        {
            return new SpecResponse
            {
                Id = spec.Id,
                Title = spec.Title,
                CategoryId = spec.CategoryId
            };
        }

        public static SpecsResponse MapToResponse(this IEnumerable<Spec> specs)
        {
            return new SpecsResponse
            {
                Specs = specs.Select(MapToResponse)
            };
        }
    }
}
