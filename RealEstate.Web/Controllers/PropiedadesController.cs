using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Application.Services.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.EnumerablesModel;
using RealEstate.Persistance.Models.ViewModel;
using RealEstate.Web.Helpers.Imagenes;

namespace RealEstate.Web.Controllers
{
    public class PropiedadesController : Controller
    {
        private readonly IPropiedadesService _propiedadesService;
        private readonly IPropiedadFotosService _propiedadFotosService;
        private readonly IOfertasService _ofertasService;
        private readonly ImagenHelper _imagenHelper;
        private readonly IUsuariosService _usuariosService;

        public PropiedadesController(IPropiedadesService propiedadesService,
                                     ImagenHelper imagenHelper,
                                     IPropiedadFotosService propiedadFotosService,
                                     IUsuariosService usuariosService,
                                     IOfertasService ofertasService)
        {
            _propiedadesService = propiedadesService;
            _imagenHelper = imagenHelper;
            _propiedadFotosService = propiedadFotosService;
            _ofertasService = ofertasService;
        }

        public async Task <IActionResult> Index()
        {
            var result = await _propiedadesService.GetAllAsync();

            if (result.IsSuccess)
            {
                List<PropiedadesModel> propiedades = (List<PropiedadesModel>)result.Model;
                return View(propiedades);
            }
            return View();
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

        // Aqui se esta trabajando
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

        public ActionResult Create()
        {
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
                //if (dto.Imagen != null && dto.PropiedadID > 0)
                //{
                //    await _propiedadesService.UpdateAsync(dto);
                //}

                //if (!result.IsSuccess)
                //{
                //    result.IsSuccess = false;
                //    ViewBag.Message = result.Messages;

                //    return View(dto);
                //}

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

        public async Task <IActionResult> Edit(int id)
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
        public async Task <IActionResult> Edit(PropiedadesDto dto)
        {
            try
            {
                var result = await _propiedadesService.UpdateAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Propiedad editada exitosamente.";
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
