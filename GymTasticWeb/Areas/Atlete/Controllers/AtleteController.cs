using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace GymTasticWeb.Areas.Atlete.Controllers
{
    [Area("Atlete")]
    [Authorize(Roles = "Atlete")]
    public class AtleteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AtleteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var atleteResult = _unitOfWork.Atlete.Get(i => i.UserId == userId);
            if (atleteResult == null)
            {
                return NotFound();
            }

            AtleteViewModel atleteViewModel = new AtleteViewModel
            {
                Atlete = atleteResult,
                GenderList = _unitOfWork.Gender.GetAll().Select(u => new SelectListItem
                {
                    Text = u.GenderDescription,
                    Value = u.Id.ToString()
                }),
                AttachmentList = _unitOfWork.Attachment.GetAll()
                                    .Where(u => u.AtleteId == atleteResult.Id)
                                    .ToList(),
                FileClassificationTypeList = _unitOfWork.FileClassificationType.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Description,
                    Value = u.Id.ToString()
                })
            };

            return View(atleteViewModel);
        }


        [HttpPost]
        public IActionResult Edit(AtleteViewModel atleteViewModel, IFormFile? file)
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
                atleteViewModel.Atlete.UserId = userId;

                _unitOfWork.Atlete.Update(atleteViewModel.Atlete);
                _unitOfWork.Save();

                TempData["success"] = "Atleta atualizado com sucesso.";
                return RedirectToAction("Index", "Home");
            }

            return View(atleteViewModel); // também pode ser útil reatribuir os dropdowns aqui
        }

    }
}
