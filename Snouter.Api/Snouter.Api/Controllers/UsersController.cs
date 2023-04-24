using Microsoft.AspNetCore.Mvc;
using Snouter.Api.Mapping;
using Snouter.Application.Services;
using Snouter.Contracts.Requests;

namespace Snouter.Api.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route(ApiEndpoints.User.Create)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var user = request.MapToUser();


            await _userService.CreateAsync(user);

            var response = user.MapToResponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpGet]
        [Route(ApiEndpoints.User.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();

            var response = users.MapToResponse();

            return Ok(response);
        }

        [HttpGet]
        [Route(ApiEndpoints.User.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            var response = user.MapToResponse();

            return Ok(response);
        }

        [HttpPut]
        [Route(ApiEndpoints.User.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
        {
            var user = request.MapToUser(id);
            var updatedUser = await _userService.UpdateAsync(user);

            if (updatedUser is null)
            {
                return NotFound();
            }

            var response = user.MapToResponse();
            return Ok(response);

        }

        [HttpDelete]
        [Route(ApiEndpoints.User.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var isDeleted = await _userService.DeleteByIdAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }


    }
}
