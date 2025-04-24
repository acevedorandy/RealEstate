using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.ViewModel;
using RealEstate.Web.Middlewares;

namespace RealEstate.Web.Controllers
{
    [ServiceFilter(typeof(LoginAuthorize))]
    [Authorize]
    public class MensajesController : Controller
    {
        private readonly IMensajesService _mensajesService;

        public MensajesController(IMensajesService mensajesService)
        { 
            _mensajesService = mensajesService;
        }


        [ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> ChatsCliente()
        {
            var result = await _mensajesService.GetChatsByClientAsync();

            if (result.IsSuccess)
            {
                List<MensajesViewModel> mensajes = (List<MensajesViewModel>)result.Model;
                return View(mensajes);
            }
            return View();
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> ObtenerConversacion(int propiedadId, string destinatarioId)
        {
            var result = await _mensajesService.GetConversationAsync(propiedadId, destinatarioId);

            if (result.IsSuccess)
            {
                List<MensajesViewModel> mensajes = (List<MensajesViewModel>)result.Model;
                return View(mensajes);
            }
            return View();
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public async Task<IActionResult> Enviar(MensajesDto dto)
        {
            var result = await _mensajesService.SendFirstMessage(dto);

            if (result.IsSuccess)
            {
                return RedirectToAction("ObtenerConversacion", "Mensajes", new { destinatarioId = dto.DestinatarioID, propiedadId = dto.PropiedadID });
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages;
                return RedirectToAction("ObtenerConversacion", "Mensajes", new { destinatarioId = dto.DestinatarioID, propiedadId = dto.PropiedadID });
            }
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MensajesDto dto)
        {
            try
            {
                var result = await _mensajesService.SendFirstMessage(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Mensaje enviado exitosamente.";
                    return RedirectToAction("Details", "Clientes", new { id = dto.PropiedadID });
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

        /* Region del Agente */

        [ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Agente")]
        public async Task<IActionResult> ChatsAgente()
        {
            var result = await _mensajesService.GetChatsByAgentAsync();

            if (result.IsSuccess)
            {
                List<MensajesViewModel> mensajes = (List<MensajesViewModel>)result.Model;
                return View(mensajes);
            }
            return View();
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Agente")]
        public async Task<IActionResult> ObtenerConversacionAgente(int propiedadId, string remitenteId, string destinatarioId)
        {
            var result = await _mensajesService.GetConversationAsAgentAsync(propiedadId, remitenteId, destinatarioId);

            if (result.IsSuccess)
            {
                List<MensajesViewModel> mensajes = (List<MensajesViewModel>)result.Model;
                return View(mensajes);
            }
            return View();
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Agente")]
        [HttpPost]
        public async Task<IActionResult> Replay(MensajesDto dto)
        {
            try
            {
                var result = await _mensajesService.SaveAsync(dto);

                if (result.IsSuccess)
                {
                    return RedirectToAction("ObtenerConversacion", "Mensajes", new { remitenteId = dto.RemitenteID, destinatarioId = dto.DestinatarioID, propiedadId = dto.PropiedadID });
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages;
                    return RedirectToAction("ObtenerConversacion", "Mensajes", new { remitenteId = dto.RemitenteID, destinatarioId = dto.DestinatarioID, propiedadId = dto.PropiedadID });
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
