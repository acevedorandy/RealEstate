

using Microsoft.AspNetCore.Identity;
using RealEstate.Application.Enum;
using RealEstate.Identity.Shared.Entities;

namespace RealEstate.Identity.Seeds
{
    public class SuperAdminUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser applicationUser = new();

            applicationUser.UserName = "superadmin";
            applicationUser.Nombre = "SuperAdmin";
            applicationUser.Apellido = "User";
            applicationUser.Email = "superadminuser@gmail.com";
            applicationUser.Cedula = "98786";
            applicationUser.EmailConfirmed = true;
            applicationUser.PhoneNumberConfirmed = true;

            if (userManager.Users.All(u => u.Id != applicationUser.Id))
            {
                var user = await userManager.FindByEmailAsync(applicationUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(applicationUser, "123");
                    await userManager.AddToRoleAsync(applicationUser, Roles.Administrador.ToString());
                    await userManager.AddToRoleAsync(applicationUser, Roles.Desarrollador.ToString());
                    await userManager.AddToRoleAsync(applicationUser, Roles.Agente.ToString());
                    await userManager.AddToRoleAsync(applicationUser, Roles.Cliente.ToString());

                }
            }
        }
    }
}
