using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Web.Controllers
{
    public class ReservasController : Controller
    {
        private readonly IReservasService _reservasService;

        public ReservasController(IReservasService reservasService)
        {
            _reservasService = reservasService;
        }

        public async Task <IActionResult> Index()
        {
            var result = await _reservasService.GetAllAsync();

            if (result.IsSuccess)
            {
                List<ReservasModel> reservas = (List<ReservasModel>)result.Model;
                return View(reservas);
            }
            return View();
        }

        public async Task <IActionResult> Details(int id)
        {
            var result = await _reservasService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                ReservasModel reservas = (ReservasModel)result.Model;
                return View(reservas);
            }
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(ReservasDto dto)
        {
            try
            {
                var result = await _reservasService.SaveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Reserva registrada exitosamente.";
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

        public async Task <IActionResult> Edit(int id)
        {
            var result = await _reservasService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                ReservasModel reservas = (ReservasModel)result.Model;
                return View(reservas);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit(ReservasDto dto)
        {
            try
            {
                var result = await _reservasService.UpdateAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Reserva actualizada exitosamente.";
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
            var result = await _reservasService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                ReservasModel reservas = (ReservasModel)result.Model;
                return View(reservas);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Delete(ReservasDto dto)
        {
            try
            {
                var result = await _reservasService.UpdateAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Reserva eliminada exitosamente.";
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
