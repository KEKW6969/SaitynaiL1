using L1.Auth.Model;
using Microsoft.AspNetCore.Identity;

namespace L1.Data;

public class AuthDbSeeder
{
    private readonly UserManager<HotelRestUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthDbSeeder(UserManager<HotelRestUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        await AddDefaultRoles();
        await AddAdminUser();
    }

    private async Task AddAdminUser()
    {
        var newAdminUser = new HotelRestUser()
        {
            UserName = "admin",
            Email = "admin@admin.com"
        };
        var existingAdminUser = await _userManager.FindByNameAsync(newAdminUser.UserName);

        if (existingAdminUser == null)
        {
            var createAdminUserResult = await _userManager.CreateAsync(newAdminUser, "VerySafePassword1!");
            if (createAdminUserResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(newAdminUser, HotelRoles.Admin);
            }
        }

    }

    private async Task AddDefaultRoles()
    {
        foreach (var role in HotelRoles.All)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
                await _roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
