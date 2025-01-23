using DemoApi.Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace DemoApi.Services
{
    public interface IRoleService
    {
        Task InitializeRolesAsync();
    }

    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public RoleService(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task InitializeRolesAsync()
        {
            foreach (var role in Enum.GetValues(typeof(Enums.Roles)).Cast<Enums.Roles>())
            {
                var roleName = StringEnum.GetStringValue(role);

                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }
        }
    }
}
