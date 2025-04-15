using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Result;
using RealEstate.Identity.Shared.Context;
using RealEstate.Identity.Shared.Entities;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Persistance.Repositories.dbo
{
    public sealed class UsuariosRepository : IUsuariosRepository
    {
        private readonly RealEstateContext _realEstateContext;
        private readonly ILogger<UsuariosRepository> _logger;
        private readonly IdentityContext _identityContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuariosRepository(RealEstateContext realEstateContext,
                                  ILogger<UsuariosRepository> logger,
                                  IdentityContext identityContext,
                                  UserManager<ApplicationUser> userManager)
        {
            _realEstateContext = realEstateContext;
            _logger = logger;
            _identityContext = identityContext;
            _userManager = userManager;
        }

        public async Task<OperationResult> ActivarOrDesactivar(string userId)
        {
            OperationResult result = new OperationResult();

            try
            {
                var usuario = await _identityContext.Users.FindAsync(userId);

                if (usuario == null)
                {
                    result.Success = false;
                    result.Message = "Usuario no encontrado.";
                    return result;
                }

                usuario.IsActive = !usuario.IsActive;

                _identityContext.Update(usuario);
                await _identityContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al procesar la solicitud: {ex.Message}";
            }
            return result;
        }

        public async Task<OperationResult> GetAgentActive()
        {
            OperationResult result = new OperationResult();

            try
            {
                var usuarios = await _identityContext.Users.ToListAsync();
                var roles = await _identityContext.Roles.ToListAsync();
                var usuarioRoles = await _identityContext.UserRoles.ToListAsync();

                var datos = (from usuario in usuarios
                             join usuarioRol in usuarioRoles on usuario.Id equals usuarioRol.UserId
                             join rol in roles on usuarioRol.RoleId equals rol.Id

                             where rol.Name == "Agente" && usuario.IsActive == true
                             orderby usuario.Nombre

                             select new UsuariosModel()
                             {
                                 Id = usuario.Id,
                                 Nombre = usuario.Nombre,
                                 Apellido = usuario.Apellido,
                                 UserName = usuario.UserName,
                                 Cedula = usuario.Cedula,
                                 Email = usuario.Email,
                                 Telefono = usuario.PhoneNumber,
                                 Rol = rol.Name,
                                 IsActive = usuario.IsActive

                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los usuarios.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async Task<OperationResult> GetAgentByName(string name)
        {
            OperationResult result = new OperationResult();

            try
            {
                var usuarios = await _identityContext.Users.ToListAsync();
                var roles = await _identityContext.Roles.ToListAsync();
                var usuarioRoles = await _identityContext.UserRoles.ToListAsync();

                var datos = (from usuario in usuarios
                             join usuarioRol in usuarioRoles on usuario.Id equals usuarioRol.UserId
                             join rol in roles on usuarioRol.RoleId equals rol.Id

                             where rol.Name == "Agente" && usuario.IsActive == true && usuario.Nombre.Contains(name)

                             select new UsuariosModel()
                             {
                                 Id = usuario.Id,
                                 Nombre = usuario.Nombre,
                                 Apellido = usuario.Apellido,
                                 UserName = usuario.UserName,
                                 Cedula = usuario.Cedula,
                                 Email = usuario.Email,
                                 Telefono = usuario.PhoneNumber,
                                 Rol = rol.Name,
                                 IsActive = usuario.IsActive

                             }).FirstOrDefault();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el agente.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async Task<OperationResult> GetIdentityUserAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                var usuarios = await _identityContext.Users.ToListAsync();
                var roles = await _identityContext.Roles.ToListAsync();
                var usuarioRoles = await _identityContext.UserRoles.ToListAsync();

                var datos = (from usuario in usuarios
                             join usuarioRol in usuarioRoles on usuario.Id equals usuarioRol.UserId
                             join rol in roles on usuarioRol.RoleId equals rol.Id

                             select new UsuariosModel()
                             {
                                 Id = usuario.Id,
                                 Nombre = usuario.Nombre,
                                 Apellido = usuario.Apellido,
                                 UserName = usuario.UserName,
                                 Cedula = usuario.Cedula,
                                 Email = usuario.Email,
                                 Telefono = usuario.PhoneNumber,
                                 Rol = rol.Name,
                                 IsActive = usuario.IsActive

                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los usuarios.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async Task<OperationResult> GetIdentityUserBy(string userId)
        {
            OperationResult result = new OperationResult();

            try
            {
                var usuarios = await _identityContext.Users.ToListAsync();
                var roles = await _identityContext.Roles.ToListAsync();
                var usuarioRoles = await _identityContext.UserRoles.ToListAsync();

                var datos = (from usuario in usuarios
                             join usuarioRol in usuarioRoles on usuario.Id equals usuarioRol.UserId
                             join rol in roles on usuarioRol.RoleId equals rol.Id

                             where usuario.Id == userId

                             select new UsuariosModel()
                             {
                                 Id = usuario.Id,
                                 Nombre = usuario.Nombre,
                                 Apellido = usuario.Apellido,
                                 UserName = usuario.UserName,
                                 Cedula = usuario.Cedula,
                                 Email = usuario.Email,
                                 Telefono = usuario.PhoneNumber,
                                 Rol = rol.Name,
                                 IsActive = usuario.IsActive

                             }).FirstOrDefault();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los usuarios.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async Task<OperationResult> GetUserByRol(string rol)
        {
            OperationResult result = new OperationResult();

            try
            {
                var usuarios = await _identityContext.Users.ToListAsync();
                var roles = await _identityContext.Roles.ToListAsync();
                var usuarioRoles = await _identityContext.UserRoles.ToListAsync();

                var datos = (from usuario in usuarios
                             join usuarioRol in usuarioRoles on usuario.Id equals usuarioRol.UserId
                             join rolDb in roles on usuarioRol.RoleId equals rolDb.Id

                             where rolDb.Name == rol

                             select new UsuariosModel()
                             {
                                 Id = usuario.Id,
                                 Nombre = usuario.Nombre,
                                 Apellido = usuario.Apellido,
                                 Cedula = usuario.Cedula,
                                 Email = usuario.Email,
                                 Telefono = usuario.PhoneNumber,
                                 Rol = rolDb.Name,
                                 IsActive = usuario.IsActive

                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los usuarios.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
