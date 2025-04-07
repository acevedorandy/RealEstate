

using Microsoft.AspNetCore.Identity;
using RealEstate.Application.Enum;
using RealEstate.Identity.Shared.Entities;

namespace RealEstate.Identity.Seeds
{
    public class DefaultAgenteUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser applicationUser = new ApplicationUser();

            applicationUser.Nombre = "agente";
            applicationUser.Apellido = "user";
            applicationUser.UserName = "agenteuser";
            applicationUser.Email = "agenteuser@gmail.com";
            applicationUser.Cedula = "123";
            applicationUser.EmailConfirmed = true;
            applicationUser.PhoneNumberConfirmed = true;

            if (userManager.Users.All(u => u.Id != applicationUser.Id))
            {
                var user = await userManager.FindByEmailAsync(applicationUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(applicationUser, "123");
                    await userManager.AddToRoleAsync(applicationUser, Roles.Agente.ToString());
                }
            }
        }
    }
}
