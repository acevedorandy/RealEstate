using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Result;
using RealEstate.Identity.Shared.Context;
using RealEstate.Persistance.Base;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;

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
                Ofertas? ofertasToUpdate = await _realEstateContext.Ofertas.FindAsync(ofertas.OfertaID);

                ofertasToUpdate.OfertaID = ofertas.OfertaID;
                ofertasToUpdate.ClienteID = ofertas.ClienteID;
                ofertasToUpdate.PropiedadID = ofertas.PropiedadID;
                ofertasToUpdate.Cifra = ofertas.Cifra;
                ofertasToUpdate.FechaOferta = ofertas.FechaOferta;
                ofertasToUpdate.Estado = ofertas.Estado;
                ofertasToUpdate.Aceptada = ofertas.Aceptada;

                result = await base.Update(ofertas);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando la oferta.";
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
    }
}
