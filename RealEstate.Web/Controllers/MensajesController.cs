using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Context;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Web.Controllers
{
    public class MensajesController : Controller
    {
        private readonly IMensajesService _mensajesService;
        private readonly RealEstateContext _realEstateContext;

        public MensajesController(IMensajesService mensajesService, RealEstateContext realEstateContext)
        { 
            _mensajesService = mensajesService;
            _realEstateContext = realEstateContext;
        }

        public async Task <IActionResult> Index()
        {
            var result = await _mensajesService.GetDestinatarioAsync();

            if (result.IsSuccess)
            {
                List<MensajesModel> mensajes = (List<MensajesModel>)result.Model; 
                return View(mensajes);
            }
            return View();
        }

        public async Task <IActionResult> Details(int id)
        {
            var result = await _mensajesService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                MensajesModel mensajes = (MensajesModel)result.Model; 
                return View(mensajes);
            }
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(MensajesDto dto)
        {
            try
            {
                var result = await _mensajesService.SaveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Mensaje enviado exitosamente.";
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

        [HttpGet]
        public async Task<IActionResult> Conversacion(string remitenteId, string destinatarioId)
        {
            var mensaje = await _mensajesService.GetConversation(remitenteId, destinatarioId);
            return PartialView("_ConversacionPartial", mensaje.Model);
        }

        public async Task <IActionResult> Edit(int id)
        {
            var result = await _mensajesService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                MensajesModel mensajes = (MensajesModel)result.Model;
                return View(mensajes);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit(MensajesDto dto)
        {
            try
            {
                var result = await _mensajesService.UpdateAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Mensaje actualizado exitosamente.";
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
            var result = await _mensajesService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                MensajesModel mensajes = (MensajesModel)result.Model;
                return View(mensajes);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Delete(MensajesDto dto)
        {
            try
            {
                var result = await _mensajesService.RemoveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Mensaje actualizado exitosamente.";
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
