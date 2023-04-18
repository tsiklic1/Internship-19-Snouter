using Microsoft.AspNetCore.Mvc;
using Snouter.Api.Mapping;
using Snouter.Application.Models;
using Snouter.Application.Repository;
using Snouter.Contracts.Requests;
using Snouter.Contracts.Responses;

namespace Snouter.Api.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        [Route(ApiEndpoints.Product.Create)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var product = request.MapToProduct();

            var isCreated = _productRepository.CreateAsync(product).Result;

            if (!isCreated)
            {
                return BadRequest();
            }

            var response = product.MapToResponse();

            return Ok(response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Product.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetAllAsync();

            var response = products.MapToResponse(products);

            return Ok(response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Product.Get)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            var response = product.MapToResponse();

            return Ok(response);
        }

    }
}
