using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymTasticWeb.Areas.Trainer.Controllers
{

    [Area("Trainer")]
    [Authorize(Roles = "Trainer")]
    public class TrainerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public TrainerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Edit()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var trainerResult = _unitOfWork.Trainer.Get(i => i.UserId == userId);
            if (trainerResult == null)
            {
                return NotFound();
            }
            GymTastic.Models.Models.Trainer trainerModel = trainerResult;

            return View(trainerModel);
        }

        [HttpPost]
        public IActionResult Edit(GymTastic.Models.Models.Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                // Recupera o ID do usuário logado
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound();
                }

                // Garante que o UserId seja corretamente atribuído
                trainer.UserId = userId;

                _unitOfWork.Trainer.Update(trainer);
                _unitOfWork.Save();

                TempData["success"] = "Treinador atualizado com sucesso.";
                return RedirectToAction("Index", "Home");
            }

            return View(trainer);

        }
    }
}
