using Microsoft.AspNetCore.Mvc.Rendering;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Enum;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Web.Helpers.Otros
{
    public class SelectListHelper
    {
        private readonly ITiposPropiedadService _tiposPropiedadService;
        private readonly ITiposVentaService _tiposVentaService;
        private readonly IPropiedadesService _propiedadesService;

        public SelectListHelper(ITiposPropiedadService tiposPropiedadService,
                                ITiposVentaService tiposVentaService,
                                IPropiedadesService propiedadesService)
        {
            _tiposPropiedadService = tiposPropiedadService;
            _tiposVentaService = tiposVentaService;
            _propiedadesService = propiedadesService;
        }

        public List<SelectListItem> Roles()
        {
            return Enum.GetValues(typeof(ClienteAgente))
                .Cast<ClienteAgente>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                }).ToList();
        }

        public async Task<List<SelectListItem>> GetPropertyAvailableByAgent()
        {
            var cuentasList = new List<SelectListItem>();

            try
            {
                var response = await _propiedadesService.GetAllPropertyByAgentLogged();

                if (response.IsSuccess && response.Model is List<PropiedadesModel> Propiedad)
                {
                    cuentasList = Propiedad.Select(t => new SelectListItem
                    {
                        Text = t.Titulo.ToString(),
                        Value = t.PropiedadID.ToString()

                    }).ToList();
                }
            }
            catch (Exception)
            {
            }
            return cuentasList;
        }

        public async Task<List<SelectListItem>> GetPropertyTypes()
        {
            var cuentasList = new List<SelectListItem>();

            try
            {
                var response = await _tiposPropiedadService.GetAllAsync();

                if (response.IsSuccess && response.Model is List<TiposPropiedadModel> tiposPropiedad)
                {
                    cuentasList = tiposPropiedad.Select(t => new SelectListItem
                    {
                        Text = t.Nombre.ToString(),
                        Value = t.TipoPropiedadID.ToString()

                    }).ToList();
                }
            }
            catch (Exception)
            {
            }
            return cuentasList;
        }

        public async Task<List<SelectListItem>> GetSellingTypes()
        {
            var cuentasList = new List<SelectListItem>();

            try
            {
                var response = await _tiposVentaService.GetAllAsync();

                if (response.IsSuccess && response.Model is List<TiposVentaModel> tiposVenta)
                {
                    cuentasList = tiposVenta.Select(t => new SelectListItem
                    {
                        Text = t.Nombre.ToString(),
                        Value = t.TipoVentaID.ToString()

                    }).ToList();
                }
            }
            catch (Exception)
            {
            }
            return cuentasList;
        }
    }
}
