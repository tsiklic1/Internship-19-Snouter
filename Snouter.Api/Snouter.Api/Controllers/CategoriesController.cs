using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken token)
        {
            var category = request.MapToCategory();


            await _categoryService.CreateAsync(category, token);

            var response = category.MapToResponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Category.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var categories = await _categoryService.GetAllAsync(token);

            var response = categories.MapToResponse();

            return Ok(response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Category.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
        {
            var category = await _categoryService.GetByIdAsync(id, token);

            if (category is null)
            {
                return NotFound();
            }

            var response = category.MapToResponse();

            return Ok(response);
        }

        [HttpPut]
        [Route(ApiEndpoints.Category.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken token)
        {
            var category = request.MapToCategory(id);
            var updatedCategory = await _categoryService.UpdateAsync(category, token);

            if (updatedCategory is null) 
            {
                return NotFound();
            }

            var response = category.MapToResponse();
            return Ok(response);

        }

        [HttpDelete]
        [Route(ApiEndpoints.Category.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var isDeleted = await _categoryService.DeleteByIdAsync(id, token);
            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
