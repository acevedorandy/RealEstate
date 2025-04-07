using Microsoft.AspNetCore.Identity;
using RealEstate.Application.Enum;
using RealEstate.Identity.Shared.Entities;


namespace RealEstate.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrador.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Desarrollador.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Agente.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Cliente.ToString()));

        }
    }
}
