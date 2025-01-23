using DemoApi.Common.Result;
using DemoApi.Model.Api;
using DemoApi.Model.User;
using DemoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("")]
        public async Task<IActionResult> List(GetPagingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<dynamic>
                {
                    Status = false,
                    Message = "Failure",
                    Data = ModelState
                });
            }

            var result = await _userService.GetPagings(request);

            return Ok(new ApiResponse<PagedResult<UserDTO>>
            {
                Status = true,
                Message = "Success",
                Data = result
            });
        }
    }
}
