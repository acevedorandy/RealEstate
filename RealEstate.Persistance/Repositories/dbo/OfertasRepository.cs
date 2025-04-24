using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;
using RealEstate.Identity.Shared.Context;
using RealEstate.Persistance.Base;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.ViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace RealEstate.Persistance.Repositories.dbo
{
    public sealed class OfertasRepository(RealEstateContext realEstateContext, IdentityContext identityContext,
                                          ILogger<OfertasRepository> logger) : BaseRepository<Ofertas>(realEstateContext), IOfertasRepository
    {
        private readonly RealEstateContext _realEstateContext = realEstateContext;
        private readonly IdentityContext _identityContext = identityContext;
        private readonly ILogger<OfertasRepository> _logger = logger;

        public async override Task<OperationResult> Save(Ofertas ofertas)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Save(ofertas);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando la oferta.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(Ofertas ofertas)
        {
            OperationResult result = new OperationResult();

            try
            {
                Ofertas? ofertaToUpdate = await _realEstateContext.Ofertas.FindAsync(ofertas.OfertaID);

                if (ofertaToUpdate == null)
                {
                    result.Success = false;
                    result.Message = "Oferta no encontrada.";
                    return result;
                }

                ofertaToUpdate.Estado = ofertas.Estado;

                result = await base.Update(ofertaToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el estado de la oferta.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(Ofertas ofertas)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Remove(ofertas);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error elimninando la oferta.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                var clientes = await _identityContext.Users
                    .ToListAsync();

                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var ofertas = await _realEstateContext.Ofertas
                    .ToArrayAsync();

                var datos = (from oferta in ofertas
                             join cliente in clientes on oferta.ClienteID equals cliente.Id
                             join propiedad in propiedades on oferta.PropiedadID equals propiedad.PropiedadID

                             select new OfertasModel
                             {
                                 OfertaID = oferta.OfertaID,
                                 ClienteID = cliente.Id,
                                 PropiedadID = propiedad.PropiedadID,
                                 Cifra = oferta.Cifra,
                                 FechaOferta = oferta.FechaOferta,
                                 Estado = oferta.Estado,
                                 Aceptada = oferta.Aceptada

                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo las ofertas";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
        public async override Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {
                var clientes = await _identityContext.Users
                    .ToListAsync();

                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var ofertas = await _realEstateContext.Ofertas
                    .ToArrayAsync();

                var datos = (from oferta in ofertas
                             join cliente in clientes on oferta.ClienteID equals cliente.Id
                             join propiedad in propiedades on oferta.PropiedadID equals propiedad.PropiedadID

                             where oferta.OfertaID == id

                             select new OfertasModel
                             {
                                 OfertaID = oferta.OfertaID,
                                 ClienteID = cliente.Id,
                                 PropiedadID = propiedad.PropiedadID,
                                 Cifra = oferta.Cifra,
                                 FechaOferta = oferta.FechaOferta,
                                 Estado = oferta.Estado,
                                 Aceptada = oferta.Aceptada

                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo la oferta";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async Task<OperationResult> GetPropertyOffered(string clienteId)
        {
            OperationResult result = new OperationResult();

            try
            {
                var clientes = await _identityContext.Users.ToListAsync();
                var agentes = clientes;
                var propiedades = await _realEstateContext.Propiedades.ToListAsync();
                var ofertas = await _realEstateContext.Ofertas.ToArrayAsync();

                var datos = (from oferta in ofertas
                             join cliente in clientes on oferta.ClienteID equals cliente.Id
                             join propiedad in propiedades on oferta.PropiedadID equals propiedad.PropiedadID
                             join agente in agentes on propiedad.AgenteID equals agente.Id

                             where oferta.ClienteID == clienteId

                             select new OfertasViewModel
                             {
                                 OfertaID = oferta.OfertaID,
                                 ClienteID = cliente.Id,
                                 PropiedadID = propiedad.PropiedadID,
                                 Codigo = propiedad.Codigo,
                                 Titulo = propiedad.Titulo,
                                 TipoPropiedad = propiedad.TipoPropiedad,
                                 Imagen = propiedad.Imagen,
                                 NombreAgente = agente.Nombre,
                                 Cifra = oferta.Cifra,
                                 FechaOferta = oferta.FechaOferta,
                                 Estado = oferta.Estado,
                                 Aceptada = oferta.Aceptada

                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo la oferta";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async Task<OperationResult> GetOfferedByMyProperty(int propiedadId)
        {
            OperationResult result = new OperationResult();

            try
            {
                var clientes = await _identityContext.Users.ToListAsync();
                var propiedades = await _realEstateContext.Propiedades.ToListAsync();
                var ofertas = await _realEstateContext.Ofertas.ToArrayAsync();

                var datos = (from oferta in ofertas
                             join cliente in clientes on oferta.ClienteID equals cliente.Id
                             join propiedad in propiedades on oferta.PropiedadID equals propiedad.PropiedadID
                             where propiedad.PropiedadID == propiedadId

                             select new OfertasViewModel
                             {
                                 OfertaID = oferta.OfertaID,
                                 ClienteID = cliente.Id,
                                 PropiedadID = propiedad.PropiedadID,
                                 Codigo = propiedad.Codigo,
                                 Imagen = propiedad.Imagen,
                                 NombreCliente = cliente.Nombre,
                                 ApellidoCliente = cliente.Apellido,
                                 FotoCliente = cliente.Foto,
                                 FechaOferta = oferta.FechaOferta,
                                 Estado = oferta.Estado,
                             })
                            .GroupBy(x => x.ClienteID)
                            .Select(g => g.First()) 
                            .ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo las ofertas";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async Task<bool> PendingBids(string clienteId)
        {
            return await _realEstateContext.Ofertas
                .AnyAsync(o => o.ClienteID == clienteId &&
               (o.Estado.ToLower() == "pendiente"));
        }

        public async Task<OperationResult> GetAllOffersByClient(int propiedadId, string clienteId)
        {
            OperationResult result = new OperationResult();

            try
            {
                var clientes = await _identityContext.Users.ToListAsync();
                var propiedades = await _realEstateContext.Propiedades.ToListAsync();
                var ofertas = await _realEstateContext.Ofertas.ToArrayAsync();

                var datos = (from oferta in ofertas
                             join cliente in clientes on oferta.ClienteID equals cliente.Id
                             join propiedad in propiedades on oferta.PropiedadID equals propiedad.PropiedadID

                             where oferta.PropiedadID == propiedadId && oferta.ClienteID == clienteId

                             select new OfertasViewModel
                             {
                                 OfertaID = oferta.OfertaID,
                                 ClienteID = cliente.Id,
                                 PropiedadID = propiedad.PropiedadID,
                                 Codigo = propiedad.Codigo,
                                 Cifra = oferta.Cifra,
                                 FechaOferta = oferta.FechaOferta,
                                 Estado = oferta.Estado,
                                 Aceptada = oferta.Aceptada
                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo las ofertas";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async Task<OperationResult> GetAllExceptId(int ofertaId)
        {
            OperationResult result = new OperationResult();

            try
            {
                var clientes = await _identityContext.Users
                    .ToListAsync();

                var propiedades = await _realEstateContext.Propiedades
                    .ToListAsync();

                var ofertas = await _realEstateContext.Ofertas
                    .ToArrayAsync();

                var datos = (from oferta in ofertas
                             join cliente in clientes on oferta.ClienteID equals cliente.Id
                             join propiedad in propiedades on oferta.PropiedadID equals propiedad.PropiedadID

                             where oferta.OfertaID != ofertaId

                             select new OfertasModel
                             {
                                 OfertaID = oferta.OfertaID,
                                 ClienteID = cliente.Id,
                                 PropiedadID = propiedad.PropiedadID,
                                 Cifra = oferta.Cifra,
                                 FechaOferta = oferta.FechaOferta,
                                 Estado = oferta.Estado,
                                 Aceptada = oferta.Aceptada

                             }).ToList();

                result.Data = datos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo las ofertas";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
