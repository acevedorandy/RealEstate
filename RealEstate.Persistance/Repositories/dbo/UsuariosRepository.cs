using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Result;
using RealEstate.Identity.Shared.Context;
using RealEstate.Identity.Shared.Entities;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.ViewModel;

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

                usuario.EmailConfirmed = !usuario.EmailConfirmed;
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

        public async Task<OperationResult> GetAllAgent()
        {
            OperationResult result = new OperationResult();

            try
            {
                var usuarios = await _identityContext.Users.ToListAsync();
                var roles = await _identityContext.Roles.ToListAsync();
                var usuarioRoles = await _identityContext.UserRoles.ToListAsync();
                var propiedades = await _realEstateContext.Propiedades.ToListAsync();

                var propiedadesPorAgente = propiedades
                    .GroupBy(p => p.AgenteID)
                    .ToDictionary(g => g.Key, g => g.Count());

                var datos = (from usuario in usuarios
                             join usuarioRol in usuarioRoles on usuario.Id equals usuarioRol.UserId
                             join rol in roles on usuarioRol.RoleId equals rol.Id

                             where rol.Name == "Agente" 

                             orderby usuario.Nombre
                             select new AgentesModel()
                             {
                                 Id = usuario.Id,
                                 Nombre = usuario.Nombre,
                                 Apellido = usuario.Apellido,
                                 Correo = usuario.Email,
                                 PropiedadID = propiedadesPorAgente.ContainsKey(usuario.Id) ? propiedadesPorAgente[usuario.Id] : 0,
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

        public async Task<OperationResult> GetAllDeveloper()
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

                             where rol.Name == "Desarrollador"

                             orderby usuario.Nombre
                             select new DesarrolladorModel()
                             {
                                 Id = usuario.Id,
                                 Nombre = usuario.Nombre,
                                 Apellido = usuario.Apellido,
                                 UserName = usuario.UserName,
                                 Cedula = usuario.Cedula,
                                 Correo = usuario.Email,
                                 IsActive = usuario.IsActive,

                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los usuarios desarrolladores.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async Task<OperationResult> GetAllAdmins()
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

                             where rol.Name == "Administrador"

                             orderby usuario.Nombre
                             select new AdministradorModel()
                             {
                                 Id = usuario.Id,
                                 Nombre = usuario.Nombre,
                                 Apellido = usuario.Apellido,
                                 UserName = usuario.UserName,
                                 Cedula = usuario.Cedula,
                                 Correo = usuario.Email,
                                 IsActive = usuario.IsActive,

                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los usuarios administradores.";
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

        public async Task<OperationResult> RemoveAgentWithProperty(string userId)
        {
            OperationResult result = new OperationResult();

            OperationResult SetError(string errorMessage)
            {
                result.Success = false;
                result.Message = errorMessage;
                return result;
            }

            using var transaction = await _realEstateContext.Database.BeginTransactionAsync();
            try
            {
                var propiedades = await _realEstateContext.Propiedades
                    .Where(p => p.AgenteID == userId)
                    .ToListAsync();

                var propiedadIds = propiedades.Select(p => p.PropiedadID).ToList();

                if (propiedadIds.Any())
                {
                    var ofertas = await _realEstateContext.Ofertas
                        .Where(o => propiedadIds.Contains(o.PropiedadID))
                        .ToListAsync();
                    _realEstateContext.Ofertas.RemoveRange(ofertas);

                    var fotos = await _realEstateContext.PropiedadFotos
                        .Where(pf => propiedadIds.Contains(pf.PropiedadID))
                        .ToListAsync();
                    _realEstateContext.PropiedadFotos.RemoveRange(fotos);

                    var mejoras = await _realEstateContext.PropiedadMejoras
                        .Where(m => propiedadIds.Contains(m.PropiedadID))
                        .ToListAsync();
                    _realEstateContext.PropiedadMejoras.RemoveRange(mejoras);

                    _realEstateContext.Propiedades.RemoveRange(propiedades);
                    await _realEstateContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                var usuario = await _userManager.FindByIdAsync(userId);
                if (usuario == null)
                    return SetError("Usuario no encontrado.");

                var identityResult = await _userManager.DeleteAsync(usuario);

                if (!identityResult.Succeeded)
                {
                    var errorMsg = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                    return SetError($"Error al eliminar el usuario: {errorMsg}");
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el usuario.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> UpdateIdentityUser(ApplicationUser user)
        {
            OperationResult result = new OperationResult();

            OperationResult SetError(string message)
            {
                result.Success = false;
                result.Message = message;
                return result;
            }

            try
            {
                var userToUpdate = await _identityContext.Users.FindAsync(user.Id);

                if (userToUpdate == null)
                    return SetError("Usuario no encontrado.");

                userToUpdate.Nombre = user.Nombre;
                userToUpdate.Apellido = user.Apellido;
                userToUpdate.Cedula = user.Cedula;
                userToUpdate.Email = user.Email;
                userToUpdate.UserName = user.UserName;

                if (!string.IsNullOrWhiteSpace(user.PasswordHash))
                {
                    userToUpdate.PasswordHash = _userManager.PasswordHasher.HashPassword(userToUpdate, user.PasswordHash);
                }

                var updateResult = await _userManager.UpdateAsync(userToUpdate);

                if (!updateResult.Succeeded)
                {
                    var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
                    return SetError("Error actualizando usuario: " + errors);
                }

                result.Success = true;
                result.Message = "Usuario actualizado correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el usuario.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }
    }
}

