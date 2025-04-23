using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Application.Models;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.EnumerablesModel;
using RealEstate.Persistance.Models.ViewModel;
using RealEstate.Web.Helpers.Imagenes;
using RealEstate.Web.Helpers.Otros;
using RealEstate.Web.Middlewares;

namespace RealEstate.Web.Controllers
{
    [ServiceFilter(typeof(LoginAuthorize))]
    [Authorize(Roles = "Agente")]
    public class PropiedadesController : Controller
    {
        private readonly IPropiedadesService _propiedadesService;
        private readonly IPropiedadFotosService _propiedadFotosService;
        private readonly IOfertasService _ofertasService;
        private readonly ImagenHelper _imagenHelper;
        private readonly SelectListHelper _selectListHelper;
        private readonly IMejorasService _mejorasService;
        private readonly IPropiedadMejorasService _propiedadMejorasService;

        public PropiedadesController(IPropiedadesService propiedadesService,
                                     ImagenHelper imagenHelper,
                                     IPropiedadFotosService propiedadFotosService,
                                     IUsuariosService usuariosService,
                                     IOfertasService ofertasService,
                                     SelectListHelper selectListHelper,
                                     IMejorasService mejorasService,
                                     IPropiedadMejorasService propiedadMejorasService)
        {
            _propiedadesService = propiedadesService;
            _imagenHelper = imagenHelper;
            _propiedadFotosService = propiedadFotosService;
            _ofertasService = ofertasService;
            _selectListHelper = selectListHelper;
            _mejorasService = mejorasService;
            _propiedadMejorasService = propiedadMejorasService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _propiedadesService.GetAllPropertyByAgentLogged();

            if (result.IsSuccess)
            {
                List<PropiedadesModel> propiedades = (List<PropiedadesModel>)result.Model;
                return View(propiedades);
            }
            return View();
        }

        public async Task<IActionResult> Mejoras()
        {
            var propiedades = await _selectListHelper.GetPropertyAvailableByAgent();
            ViewBag.Propiedad = propiedades;

            var result = await _mejorasService.GetAllAsync();

            if (result.IsSuccess)
            {
                List<MejorasModel> mejoras = (List<MejorasModel>)result.Model;
                return View(mejoras);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AsignarMejora(PropiedadMejorasDto dto)
        {
            try
            {
                var result = await _propiedadMejorasService.SaveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Mejora añadida exitosamente.";
                    return RedirectToAction("Mejoras", "Propiedades");
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages;
                    return RedirectToAction("Mejoras", "Propiedad");
                }
            }
            catch
            {
                return View();
            }
        }

        public async Task <IActionResult> Details(int id)
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

        public async Task<IActionResult> PropertyOffers(int id)
        {
            var result = await _ofertasService.GetOfferedByMyPropertyAsync(id);

            if (result.IsSuccess)
            {
                List<OfertasViewModel> ofertas = (List<OfertasViewModel>)result.Model;
                return View(ofertas);
            }
            return View();
        }

        public async Task<IActionResult> OffersByUser(int propiedadId, string clienteId)
        {
            var result = await _ofertasService.GetAllOffersByClientAsync(propiedadId, clienteId);

            if (result.IsSuccess)
            {
                List<OfertasViewModel> ofertas = (List<OfertasViewModel>)result.Model;
                return View(ofertas);
            }
            return View();
        }

        public async Task <ActionResult> Create()
        {
            var tiposPropiedad = await _selectListHelper.GetPropertyTypes();
            var tipoVenta = await _selectListHelper.GetSellingTypes();

            ViewBag.TiposPropiedad = tiposPropiedad;
            ViewBag.TiposVenta = tipoVenta;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(PropiedadesDto dto)
        {
            try
            {
                var result = await _propiedadesService.SaveAsync(dto);

                if (result.IsSuccess)
                {
                    dto = result.Model;
                    var saveFoto = await _imagenHelper.SavePropertyPhotos(dto);

                    if (saveFoto.Any())
                    {
                        dto.Imagen = saveFoto.First();
                        await _propiedadesService.UpdateAsync(dto);
                    }

                    var photosResponse = await _propiedadFotosService.AddPhotoAsEntity(dto.PropiedadID, saveFoto);

                    if (!photosResponse.IsSuccess)
                    {
                        TempData["WarningMessage"] = "Propiedad guardada pero hubo problemas con algunas fotos";
                    }

                    TempData["SuccessMessage"] = "Propiedad agregada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages;
                    var tiposPropiedad = await _selectListHelper.GetPropertyTypes();
                    var tipoVenta = await _selectListHelper.GetSellingTypes();

                    ViewBag.TiposPropiedad = tiposPropiedad;
                    ViewBag.TiposVenta = tipoVenta;
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> AdministrarMejoras(int id)
        {
            var result = await _mejorasService.GetMejorasByPropertyAsync(id);

            if (result.IsSuccess)
            {
                List<PropiedadMejorasModelViewModel> mejoras = (List<PropiedadMejorasModelViewModel>)result.Model;
                return View(mejoras);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EliminarPropiedadMejora(int PropiedadMejoraID)
        {
            try
            {
                var dto = new PropiedadMejorasDto { PropiedadMejoraID = PropiedadMejoraID };
                var result = await _propiedadMejorasService.RemoveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Mejora eliminada exitosamente.";
                    return RedirectToAction("AdministrarMejoras", "Propiedades");
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages;
                    return RedirectToAction("AdministrarMejoras", "Propiedades");
                }
            }
            catch
            {
                return View();
            }
        }

        public async Task <IActionResult> Edit(int id)
        {
            var tipoPropiedad = await _selectListHelper.GetPropertyTypes();
            var tipoVenta = await _selectListHelper.GetSellingTypes();

            ViewBag.TiposPropiedad = tipoPropiedad;
            ViewBag.TiposVenta = tipoVenta;

            var result = await _propiedadesService.LoadPropertyAsync(id);

            if (result.IsSuccess)
            {
                PropiedadesViewModel propiedades = (PropiedadesViewModel)result.Model;
                return View(propiedades);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PropiedadesDto dto)
        {
            try
            {
                var result = await _propiedadesService.UpdateAsync(dto);

                if (result.IsSuccess)
                {
                    dto = result.Model;
                    var updateFoto = await _imagenHelper.UpdatePropertyPhoto(dto);

                    if (updateFoto.Any())
                    {
                        dto.Imagen = updateFoto.First();
                        await _propiedadesService.UpdateAsync(dto);
                        var photosResponse = await _propiedadFotosService.AddPhotoAsEntity(dto.PropiedadID, updateFoto);
                    }
                    var updateResult = await _propiedadesService.UpdateAsync(dto);

                    if (!updateResult.IsSuccess)
                    {
                        updateResult.IsSuccess = false;
                        TempData["ErrorMessage"] = updateResult.Messages;
                    }

                    TempData["SuccessMessage"] = "Propiedad editada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages;
                    var tipoPropiedad = await _selectListHelper.GetPropertyTypes();
                    var tipoVenta = await _selectListHelper.GetSellingTypes();

                    ViewBag.TiposPropiedad = tipoPropiedad;
                    ViewBag.TiposVenta = tipoVenta;
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        public async Task <IActionResult> Delete(int id)
        {
            var result = await _propiedadesService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                PropiedadesModel propiedades = (PropiedadesModel)result.Model;
                return View(propiedades);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Delete(PropiedadesDto dto)
        {
            try
            {
                var result = await _propiedadesService.RemoveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Propiedad eliminada exitosamente.";
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
    }
}
