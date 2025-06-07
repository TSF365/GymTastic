using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;
using GymTastic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

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

            AtleteViewModel atleteViewModel = new AtleteViewModel();
            atleteViewModel.Atlete = atleteResult;

            atleteViewModel.GenderList = _unitOfWork.Gender.GetAll().Select(u => new SelectListItem
            {
                Text = u.GenderDescription,
                Value = u.Id.ToString()
            });
            atleteViewModel.AttachmentList = _unitOfWork.Attachment.GetAll();
            atleteViewModel.AttachmentList = atleteViewModel.AttachmentList.Where(u => u.AtleteId == atleteResult.Id).ToList();

            atleteViewModel.FileClassificationTypeList = _unitOfWork.FileClassificationType.GetAll().Select(u => new SelectListItem
            {
                Text = u.Description,
                Value = u.Id.ToString(),
                // Selected = u.Id 
            });

            var selectedPrefs = _unitOfWork.AtletePreference
            .GetAll()
            .Where(ap => ap.Id_Atlete == atleteResult.Id)
            .Select(ap => ap.Id_Preference)
            .ToList();


            atleteViewModel.SelectedPreferenceIds = selectedPrefs;

            atleteViewModel.PreferenceList = _unitOfWork.Preference.GetAll().Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            });

            return View(atleteViewModel);
        }


        [HttpPost]
        public IActionResult Edit(AtleteViewModel atleteViewModel, IFormFile? file)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                // Recupera o ID do usuário logado
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound();
                }

                // Garante que o UserId seja corretamente atribuído
                atleteViewModel.Atlete.UserId = userId;

                _unitOfWork.Atlete.Update(atleteViewModel.Atlete);
                _unitOfWork.Save();

                // Remove existing preferences
                var existing = _unitOfWork.AtletePreference
                .GetAll()
                .Where(p => p.Id_Atlete == atleteViewModel.Atlete.Id);

                foreach (var item in existing)
                {
                    _unitOfWork.AtletePreference.Remove(item);
                }


                // Add updated preferences
                foreach (var prefId in atleteViewModel.SelectedPreferenceIds)
                {
                    var atletePref = new AtletePreferences
                    {
                        Id_Atlete = atleteViewModel.Atlete.Id,
                        Id_Preference = prefId
                    };
                    _unitOfWork.AtletePreference.Add(atletePref);
                }

                _unitOfWork.Save();


                return RedirectToAction("Index", "Home");
            }
            
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }
            atleteViewModel.Atlete.UserId = userId;

            atleteViewModel.GenderList = _unitOfWork.Gender.GetAll().Select(u => new SelectListItem
            {
                Text = u.GenderDescription,
                Value = u.Id.ToString()
            });

            atleteViewModel.AttachmentList = _unitOfWork.Attachment.GetAll()
                .Where(u => u.AtleteId == atleteViewModel.Atlete.Id)
                .ToList();

            atleteViewModel.FileClassificationTypeList = _unitOfWork.FileClassificationType.GetAll().Select(u => new SelectListItem
            {
                Text = u.Description,
                Value = u.Id.ToString()
            });
            return View(atleteViewModel);
        }

    }
}
