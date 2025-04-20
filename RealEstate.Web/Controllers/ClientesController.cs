using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.EnumerablesModel;

namespace RealEstate.Web.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IPropiedadesService _propiedadesService;
        private readonly IPropiedadFotosService _propiedadFotosService;
        private readonly IFavoritosService _favoritosService;


        public ClientesController(IPropiedadesService propiedadesService, 
                                  IPropiedadFotosService propiedadFotosService,
                                  IFavoritosService favoritosService)
        {
            _propiedadesService = propiedadesService;
            _propiedadFotosService = propiedadFotosService;
            _favoritosService = favoritosService;
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

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
