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
        public async Task<IActionResult> Create([FromBody] CreateSpecRequest request)
        {
            var spec = request.MapToSpec();


            await _specService.CreateAsync(spec);

            var response = spec.MapToResponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Spec.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var specs = await _specService.GetAllAsync();

            var response = specs.MapToResponse();

            return Ok(response);
        }

        [HttpGet]
        [Route(ApiEndpoints.Spec.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var spec = await _specService.GetByIdAsync(id);

            if (spec is null)
            {
                return NotFound();
            }

            var response = spec.MapToResponse();

            return Ok(response);
        }
    }
}
