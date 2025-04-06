using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Web.Controllers
{
    public class PagosController : Controller
    {
        private readonly IPagosService _pagosService;

        public PagosController(IPagosService pagosService)
        {
            _pagosService = pagosService;
        }

        public async Task <IActionResult> Index()
        {
            var result = await _pagosService.GetAllAsync();

            if (result.IsSuccess)
            {
                List<PagosModel> pagos = (List<PagosModel>)result.Model;
                return View(pagos);
            }
            return View();
        }

        public async Task <IActionResult> Details(int id)
        {
            var result = await _pagosService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                PagosModel pagos = (PagosModel)result.Model;
                return View(pagos);
            }
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(PagosDto dto)
        {
            try
            {
                var result = await _pagosService.SaveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Pago registrado exitosamente.";
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
            var result = await _pagosService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                PagosModel pagos = (PagosModel)result.Model;
                return View(pagos);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit(PagosDto dto)
        {
            try
            {
                var result = await _pagosService.UpdateAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Pago actualizado exitosamente.";
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
            var result = await _pagosService.GetByIDAsync(id);

            if (result.IsSuccess)
            {
                PagosModel pagos = (PagosModel)result.Model;
                return View(pagos);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Delete(PagosDto dto)
        {
            try
            {
                var result = await _pagosService.RemoveAsync(dto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Pago eliminado exitosamente.";
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
