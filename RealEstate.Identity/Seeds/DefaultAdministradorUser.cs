

using Microsoft.AspNetCore.Identity;
using RealEstate.Application.Enum;
using RealEstate.Identity.Shared.Entities;

namespace RealEstate.Identity.Seeds
{
    public class DefaultAdministradorUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser applicationUser = new ApplicationUser();

            applicationUser.Nombre = "admin";
            applicationUser.Apellido = "user";
            applicationUser.UserName = "adminuser";
            applicationUser.Email = "adminuser@gmail.com";
            applicationUser.Cedula = "00000000000";
            applicationUser.EmailConfirmed = true;
            applicationUser.PhoneNumberConfirmed = true;

            if (userManager.Users.All(u => u.Id != applicationUser.Id))
            {
                var user = await userManager.FindByEmailAsync(applicationUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(applicationUser, "123");
                    await userManager.AddToRoleAsync(applicationUser, Roles.Administrador.ToString());
                }
            }
        }
    }
}
