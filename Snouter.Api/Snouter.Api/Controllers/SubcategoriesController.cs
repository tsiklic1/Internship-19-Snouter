using Microsoft.AspNetCore.Authorization;
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

        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpPost]
        [Route(ApiEndpoints.Subcategory.Create)]
        public async Task<IActionResult> Create([FromBody] CreateSubcategoryRequest request, CancellationToken token)
        {
            var subcategory = request.MapToSubcategory();

            await _subcategoryService.CreateAsync(subcategory, token);

            var response = subcategory.MapToResponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Subcategory.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var subcategories = await _subcategoryService.GetAllAsync(token);

            var response = subcategories.MapToResponse();

            return Ok(response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Subcategory.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
        {
            var subcategory = await _subcategoryService.GetByIdAsync(id, token);

            if (subcategory is null)
            {
                return NotFound();
            }

            var response = subcategory.MapToResponse();

            return Ok(response);
        }

        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpPut]
        [Route(ApiEndpoints.Subcategory.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateSubcategoryRequest request, CancellationToken token)
        {
            var subcategory = request.MapToSubcategory(id);
            var updatedCategory = await _subcategoryService.UpdateAsync(subcategory, token);

            if (updatedCategory is null)
            {
                return NotFound();
            }

            var response = subcategory.MapToResponse();
            return Ok(response);

        }

        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpDelete]
        [Route(ApiEndpoints.Subcategory.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var isDeleted = await _subcategoryService.DeleteByIdAsync(id, token);
            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
