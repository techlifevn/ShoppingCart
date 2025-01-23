using DemoApi.Common.Helpers;
using DemoApi.Common.Result;
using DemoApi.Data.EF;
using DemoApi.Data.Entity;
using DemoApi.Model.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoApi.Services
{
    public interface IUserService
    {
        Task<Result<UserDTO>> CreateAsync(UserCreateRequest request);
        Task<Result<bool>> UpdateAsync(UserUpdateRequest request);
        Task<Result<bool>> DeleteAsync(DeleteRequest request);
        Task<PagedResult<UserDTO>> GetPagings(GetPagingRequest request);
        Task<Result<string>> SignInAsync(SignInModel model);
        Task<bool> CheckUserExists(string username);
    }

    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UserService(DataContext context,
                           UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           IConfiguration configuration,
                           RoleManager<IdentityRole<Guid>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        public async Task<bool> CheckUserExists(string username)
        {
            var exists = await _userManager.FindByNameAsync(username);

            return exists != null;
        }

        public async Task<Result<UserDTO>> CreateAsync(UserCreateRequest request)
        {
            string action = "Create new user";

            try
            {
                if (await _context.Users.AnyAsync(x => x.UserName == request.UserName.ToLower()))
                {
                    return Result<UserDTO>.Error(action, "Username already taken", new UserDTO());
                }

                if (await _context.Users.AnyAsync(x => x.Email == request.Email.ToLower()))
                {
                    return Result<UserDTO>.Error(action, "Email already taken", new UserDTO());
                }

                var user = new User
                {
                    FirstName = request.FirstName.Trim(),
                    LastName = request.LastName.Trim(),
                    FullName = request.FirstName.Trim() + " " + request.LastName.Trim(),
                    Email = request.Email.Trim(),
                    UserName = request.UserName.Trim(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false,
                    IsStatus = true,
                };

                var result = await _userManager.CreateAsync(user, request.PasswordConfirm);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(AppRole.Customer))
                    {
                        await _roleManager.CreateAsync(new IdentityRole<Guid>(AppRole.Customer));
                    }

                    await _userManager.AddToRoleAsync(user, AppRole.Customer);

                    return Result<UserDTO>.Success(action, "Created new user successfully", new UserDTO
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        FullName = user.FullName,
                        Email = user.Email,
                        UserName = user.UserName,
                        IsDeleted = user.IsDeleted,
                        IsStatus = user.IsStatus,
                    });
                }

                return Result<UserDTO>.Error(action, "Created new user failure", new UserDTO());
            }
            catch (Exception ex)
            {
                return Result<UserDTO>.Error(action, ex.Message, new UserDTO());
            }
        }

        public Task<Result<bool>> DeleteAsync(DeleteRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<UserDTO>> GetPagings(GetPagingRequest request)
        {
            request.Keyword = request.Keyword.ToLower();

            var query = _context.Users.Where(x => !x.IsDeleted &&
                (x.FullName.ToLower().Contains(request.Keyword) || x.UserName.Contains(request.Keyword) || x.Email.Contains(request.Keyword)));

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserDTO
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FullName = x.FullName,
                    Email = x.Email,
                    UserName = x.UserName,
                    IsDeleted = x.IsDeleted,
                    IsStatus = x.IsStatus
                }).ToListAsync();

            return new PagedResult<UserDTO>
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = data.Count,
                Items = data
            };
        }

        public async Task<Result<string>> SignInAsync(SignInModel model)
        {
            string action = "Đăng nhập";

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null) return Result<string>.Error(action, "Tài khoản hoặc mật khẩu không đúng");

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (!result.Succeeded)
            {
                return Result<string>.Error(action, "Tài khoản hoặc mật khẩu không đúng");
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserName", user.UserName),
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256Signature)
            );

            return Result<string>.Success(action, "Tạo token thành công", new JwtSecurityTokenHandler().WriteToken(token));
        }

        public Task<Result<bool>> UpdateAsync(UserUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
