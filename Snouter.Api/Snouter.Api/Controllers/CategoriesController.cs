﻿using Microsoft.AspNetCore.Mvc;
using Snouter.Api.Mapping;
using Snouter.Application.Repository;
using Snouter.Application.Services;
using Snouter.Contracts.Requests;

namespace Snouter.Api.Controllers
{
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        //private readonly ICategoryRepository _categoryRepository;

        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService= categoryService;
        }

        [HttpPost]
        [Route(ApiEndpoints.Category.Create)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var category = request.MapToCategory();


            await _categoryService.CreateAsync(category);

            var response = category.MapToResponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Category.GetAll)]
        public async Task<IActionResult> GetAll() 
        {
            var categories = await _categoryService.GetAllAsync();

            var response = categories.MapToResponse();

            return Ok(response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Category.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category is null)
            {
                return NotFound();
            }

            var response = category.MapToResponse();

            return Ok(response);
        }

        [HttpPut]
        [Route(ApiEndpoints.Category.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCategoryRequest request)
        {
            var category = request.MapToCategory(id);
            var updatedCategory = await _categoryService.UpdateAsync(category);

            if (updatedCategory is null) 
            {
                return NotFound();
            }

            var response = category.MapToResponse();
            return Ok(response);

        }

        [HttpDelete]
        [Route(ApiEndpoints.Category.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var isDeleted = await _categoryService.DeleteByIdAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
