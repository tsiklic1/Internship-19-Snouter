using Microsoft.AspNetCore.Mvc;
using Snouter.Api.Mapping;
using Snouter.Application.Models;
using Snouter.Application.Repository;
using Snouter.Application.Services;
using Snouter.Contracts.Requests;
using Snouter.Contracts.Responses;

namespace Snouter.Api.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Route(ApiEndpoints.Product.Create)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var product = request.MapToProduct();

            var isCreated = _productService.CreateAsync(product).Result;

            if (!isCreated)
            {
                return BadRequest();
            }

            var response = product.MapToResponse();

            return CreatedAtAction(nameof(Get), new {id = response.Id}, response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Product.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();

            var response = products.MapToResponse();

            return Ok(response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Product.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            var response = product.MapToResponse();

            return Ok(response);
        }

        [HttpDelete]
        [Route(ApiEndpoints.Product.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var isDeleted = await _productService.DeleteByIdAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }



        //[HttpPut]
        //[Route(ApiEndpoints.Product.Update)]
        //public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductRequest request)
        //{
        //    var product = request.MapToProduct(id);
        //    var isUpdated = await _productRepository.UpdateAsync(product);

        //    if (!isUpdated)
        //    {
        //        return NotFound();
        //    }

        //    var response = product.MapToResponse();
        //    return Ok(response);

        //}

    }
}
