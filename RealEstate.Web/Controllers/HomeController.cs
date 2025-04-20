using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.EnumerablesModel;


namespace RealEstate.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPropiedadesService _propiedadesService;
        private readonly IPropiedadFotosService _propiedadFotosService;

        public HomeController(IPropiedadesService propiedadesService,
                              IPropiedadFotosService propiedadFotosService)
        {
            _propiedadesService = propiedadesService;
            _propiedadFotosService = propiedadFotosService;
        }

        public async Task <IActionResult> Index()
        {
            var result = await _propiedadesService.GetAllPropertyNotSold();

            if (result.IsSuccess)
            {
                List<PropiedadesModel> propiedades = (List<PropiedadesModel>)result.Model;
                return View(propiedades);
            }
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            PropiedadDetallesModel propiedadDetalles = new PropiedadDetallesModel();

            var resultPropiedad = await _propiedadesService.GetByIDAsync(id);

            if (resultPropiedad.IsSuccess)
            {
                propiedadDetalles.PropiedadesModel = (PropiedadesModel)resultPropiedad.Model;
            }

            var resultFotos = await _propiedadFotosService.GetPhotosByPropertyAsync(id);

            if (resultFotos.IsSuccess)
            {
                propiedadDetalles.PropiedadFotosModel = (List<PropiedadFotosModel>)resultFotos.Model;
            }

            var resultAgente = await _propiedadesService.GetAgentByPropertyAsync(id);

            if (resultAgente.IsSuccess)
            {
                propiedadDetalles.UsuariosModel = (UsuariosModel)resultAgente.Model;
            }

            return View(propiedadDetalles);
        }

        public async Task<IActionResult> Filter(string tipoPropiedad, decimal? minPrice, decimal? maxPrice, int? habitacion, int? baños)
        {
            var result = await _propiedadesService.GetAllFilter(tipoPropiedad, minPrice, maxPrice, habitacion, baños);

            if (result.IsSuccess)
            {
                List<PropiedadesModel> propiedades = (List<PropiedadesModel>)result.Model;
                return View("Index", propiedades.ToList());
            }
            return View();
        }


    }
}
