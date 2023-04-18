using Microsoft.Extensions.Hosting;
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
                Location = request.Location,
                Properties = request.Properties,

            };
        }

        public static ProductResponse MapToResponse(this Product product)
        {
            return new ProductResponse
            {
                Id = Guid.NewGuid(),
                Title = product.Title,
                IsSold = product.IsSold,
                PriceInCents = product.PriceInCents,
                Category = product.Category,
                SubCategory = product.SubCategory,
                Images = product.Images,
                Location = product.Location,
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

    }
}
