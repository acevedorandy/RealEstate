using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Services.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.EnumerablesModel;
using RealEstate.Persistance.Models.ViewModel;
using RealEstate.Web.Helpers.Otros;


namespace RealEstate.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPropiedadesService _propiedadesService;
        private readonly IPropiedadFotosService _propiedadFotosService;
        private readonly IMejorasService _mejorasService;
        private readonly SelectListHelper _selectListHelper;

        public HomeController(IPropiedadesService propiedadesService,
                              IPropiedadFotosService propiedadFotosService,
                              IMejorasService mejorasService,
                              SelectListHelper selectListHelper)
        {
            _propiedadesService = propiedadesService;
            _propiedadFotosService = propiedadFotosService;
            _mejorasService = mejorasService;
            _selectListHelper = selectListHelper;
        }

        public async Task <IActionResult> Index()
        {
            var tipos = await _selectListHelper.GetPropertyTypes();
            ViewBag.Tipos = tipos;

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

            var resultMejora = await _mejorasService.GetMejorasByPropertyAsync(id);

            if (resultMejora.IsSuccess)
            {
                propiedadDetalles.MejorasModels = (List<PropiedadMejorasModelViewModel>)resultMejora.Model;
            }

            return View(propiedadDetalles);
        }

        public async Task<IActionResult> Filter(int tipoPropiedad, decimal? minPrice, decimal? maxPrice, int? habitacion, int? baños)
        {
            var result = await _propiedadesService.GetAllFilter(tipoPropiedad, minPrice, maxPrice, habitacion, baños);

            var tipos = await _selectListHelper.GetPropertyTypes();
            ViewBag.Tipos = tipos;

            if (result.IsSuccess)
            {
                List<PropiedadesModel> propiedades = (List<PropiedadesModel>)result.Model;
                return View("Index", propiedades.ToList());
            }
            return View();
        }


    }
}
