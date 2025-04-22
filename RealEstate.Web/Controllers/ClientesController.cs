using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.EnumerablesModel;
using RealEstate.Persistance.Models.ViewModel;
using RealEstate.Web.Helpers.Otros;

namespace RealEstate.Web.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IPropiedadesService _propiedadesService;
        private readonly IPropiedadFotosService _propiedadFotosService;
        private readonly IFavoritosService _favoritosService;
        private readonly IMejorasService _mejorasService;
        private readonly SelectListHelper _selectListHelper;


        public ClientesController(IPropiedadesService propiedadesService, 
                                  IPropiedadFotosService propiedadFotosService,
                                  IFavoritosService favoritosService,
                                  IMejorasService mejorasService,
                                  SelectListHelper selectListHelper)
        {
            _propiedadesService = propiedadesService;
            _propiedadFotosService = propiedadFotosService;
            _favoritosService = favoritosService;
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

        public async Task<IActionResult> Filter(int? propiedadId, decimal? minPrice, decimal? maxPrice, int? habitacion, int? baños)
        {
            var result = await _propiedadesService.GetAllFilter(propiedadId, minPrice, maxPrice, habitacion, baños);

            var tipos = await _selectListHelper.GetPropertyTypes();
            ViewBag.Tipos = tipos;

            if (result.IsSuccess)
            {
                List<PropiedadesModel> propiedades = (List<PropiedadesModel>)result.Model;
                return View("Index", propiedades.ToList());
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Favorite(FavoritosDto dto)
        {
            try
            {
                var existeRelacion = await _favoritosService.ExistsRelationAsync(dto.PropiedadID);

                if (existeRelacion)
                {
                    TempData["ErrorMessage"] = "Al parecer esta propiedad ya se encuentra agregada en sus favoritos.";
                    return RedirectToAction("Index");
                }

                var result = await _favoritosService.SaveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Propiedad añadida a favoritos exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages;
                    return View(dto);
                }
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> MisPropiedades()
        {
            var result = await _favoritosService.GetAllFavoritePropertyByUserAsync();

            if (result.IsSuccess)
            {
                List<PropiedadesModel> propiedades = (List<PropiedadesModel>)result.Model;
                return View(propiedades);
            }
            return View();
        }
    }
}
