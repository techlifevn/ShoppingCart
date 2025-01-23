using DemoApi.Model.Api;
using DemoApi.Model.User;
using DemoApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace DemoApi.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserCreateRequest request)
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

            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?{}|<>])[a-zA-Z\d!@#$%&*()]{8,}$");

            if (!regex.IsMatch(request.PasswordConfirm))
            {
                return Ok(new ApiResponse<UserCreateRequest>
                {
                    Status = false,
                    Message = "Mật khẩu bao gồm: kí tự thường, hoa, số, đặt biệt và độ dài từ 8 kí tự",
                    Data = request
                });
            }

            if (request.Password != request.PasswordConfirm)
            {
                return Ok(new ApiResponse<UserCreateRequest>
                {
                    Status = false,
                    Message = "Mật khẩu và mật khẩu xác nhận không trùng khớp",
                    Data = request
                });
            }

            var result = await _userService.CreateAsync(request);

            if (result.IsSuccessed)
            {
                return Ok(new ApiResponse<UserDTO>
                {
                    Status = true,
                    Message = result.Message,
                    Data = result.ResultObj
                });
            }

            return Ok(new ApiResponse<UserDTO>
            {
                Status = false,
                Message = result.Message,
                Data = result.ResultObj
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(SignInModel model)
        {
            var result = await _userService.SignInAsync(model);

            if (!result.IsSuccessed)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Status = false,
                    Message = result.Message
                });
            }

            return Ok(new ApiResponse<Token>
            {
                Status = true,
                Message = result.Message,
                Data = result.ResultObj
            });
        }
    }
}
