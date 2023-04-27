﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken token)
        {
            var product = request.MapToProduct();

            //var isCreated = _productService.CreateAsync(product).Result;

            //if (!isCreated)
            //{
            //    return BadRequest();
            //}

            await _productService.CreateAsync(product, token);

            var response = product.MapToResponse();

            return CreatedAtAction(nameof(Get), new {id = response.Id}, response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Product.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var products = await _productService.GetAllAsync(token);

            var response = products.MapToResponse();

            return Ok(response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Product.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
        {
            var product = await _productService.GetByIdAsync(id, token);

            if (product is null)
            {
                return NotFound();
            }

            var response = product.MapToResponse();

            return Ok(response);
        }

        [HttpDelete]
        [Route(ApiEndpoints.Product.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var isDeleted = await _productService.DeleteByIdAsync(id, token);
            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }



        [HttpPut]
        [Route(ApiEndpoints.Product.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductRequest request, CancellationToken token)
        {
            var product = request.MapToProduct(id);
            var updatedProduct = await _productService.UpdateAsync(product, token);

            if (updatedProduct is null)
            {
                return NotFound();
            }

            var response = product.MapToResponse();
            return Ok(response);

        }

    }
}
