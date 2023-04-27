using Microsoft.AspNetCore.Mvc;
using Snouter.Api.Mapping;
using Snouter.Application.Services;
using Snouter.Contracts.Requests;

namespace Snouter.Api.Controllers
{
    [ApiController]
    public class SpecsController : ControllerBase
    {
        private readonly ISpecService _specService;
        public SpecsController(ISpecService specService)
        {
            _specService = specService;
        }

        [HttpPost]
        [Route(ApiEndpoints.Spec.Create)]
        public async Task<IActionResult> Create([FromBody] CreateSpecRequest request, CancellationToken token)
        {
            var spec = request.MapToSpec();


            await _specService.CreateAsync(spec, token);

            var response = spec.MapToResponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Spec.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var specs = await _specService.GetAllAsync(token);

            var response = specs.MapToResponse();

            return Ok(response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Spec.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
        {
            var spec = await _specService.GetByIdAsync(id, token);

            if (spec is null)
            {
                return NotFound();
            }

            var response = spec.MapToResponse();

            return Ok(response);
        }

        [HttpPut]
        [Route(ApiEndpoints.Spec.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateSpecRequest request, CancellationToken token)
        {
            var spec = request.MapToSpec(id);
            var updatedSpec = await _specService.UpdateAsync(spec, token);

            if (updatedSpec is null)
            {
                return NotFound();
            }

            var response = spec.MapToResponse();
            return Ok(response);

        }


        [HttpDelete]
        [Route(ApiEndpoints.Spec.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var isDeleted = await _specService.DeleteByIdAsync(id, token);
            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
