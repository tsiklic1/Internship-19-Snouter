using Microsoft.AspNetCore.Mvc;
using Snouter.Api.Mapping;
using Snouter.Application.Services;
using Snouter.Contracts.Requests;

namespace Snouter.Api.Controllers
{
    [ApiController]
    public class SubcategoriesController : ControllerBase
    {
        private readonly ISubcategoryService _subcategoryService;

        public SubcategoriesController(ISubcategoryService subcategoryService)
        {
            _subcategoryService = subcategoryService;
        }

        [HttpPost]
        [Route(ApiEndpoints.Subcategory.Create)]

        public async Task<IActionResult> Create([FromBody] CreateSubcategoryRequest request)
        {
            var subcategory = request.MapToSubcategory();

            await _subcategoryService.CreateAsync(subcategory);

            var response = subcategory.MapToResponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Subcategory.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var subcategories = await _subcategoryService.GetAllAsync();

            var response = subcategories.MapToResponse();

            return Ok(response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Subcategory.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var subcategory = await _subcategoryService.GetByIdAsync(id);

            if (subcategory is null)
            {
                return NotFound();
            }

            var response = subcategory.MapToResponse();

            return Ok(response);
        }


        [HttpPut]
        [Route(ApiEndpoints.Subcategory.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateSubcategoryRequest request)
        {
            var subcategory = request.MapToSubcategory(id);
            var updatedCategory = await _subcategoryService.UpdateAsync(subcategory);

            if (updatedCategory is null)
            {
                return NotFound();
            }

            var response = subcategory.MapToResponse();
            return Ok(response);

        }
    }
}
