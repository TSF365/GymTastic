using GymTastic.DataAccess.Repository;
using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;
using GymTastic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymTasticWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AtleteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AtleteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var atletesList = _unitOfWork.Atlete.GetAll(includeProperties: "Gender").ToList();
            return View(atletesList);
        }

        public IActionResult Create()
        {
            AtleteViewModel atleteViewModel = new AtleteViewModel();
            atleteViewModel.Atlete = new GymTastic.Models.Models.Atlete();
            atleteViewModel.GenderList = _unitOfWork.Gender.GetAll().Select(u => new SelectListItem
            {
                Text = u.GenderDescription,
                Value = u.Id.ToString()
            });
            atleteViewModel.PreferenceList = _unitOfWork.Preference.GetAll().Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            });

            return View(atleteViewModel);
        }

        [HttpPost]
        public IActionResult Create(AtleteViewModel atleteViewModel)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Atlete.Add(atleteViewModel.Atlete);
                _unitOfWork.Save();
                
                //save preferences
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

                return RedirectToAction(nameof(Index));
            }
            return View(atleteViewModel);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            GymTastic.Models.Models.Atlete? atleteResult = _unitOfWork.Atlete.Get(i => i.Id == id);
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
            .Where(ap => ap.Id_Atlete == id)
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
            //string oldFileNameWithPath = string.Empty;
            if (ModelState.IsValid)
            {
                // Update main athlete data
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


                return RedirectToAction("Index", "Atlete");

            }
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

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var atleteResult = _unitOfWork.Atlete.Get(u => u.Id == id);
            if (atleteResult == null)
            {
                return NotFound();
            }
            return View(atleteResult);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var atleteResult = _unitOfWork.Atlete.Get(u => u.Id == id);
            if (atleteResult == null)
            {
                return NotFound();
            }
            _unitOfWork.Atlete.Remove(atleteResult);
            _unitOfWork.Save();
            TempData["success"] = "Atleta apagado com sucesso.";
            return RedirectToAction("Index", "Atlete");

        }

        public IActionResult Preferences()
        {
            var allPreferences = _unitOfWork.Preference.GetAll().ToList();
            return View(allPreferences);
        }

        public IActionResult CreatePreference()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePreference(Preferences preferences)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Preference.Add(preferences);
                _unitOfWork.Save();
                TempData["success"] = "Preferência criada com sucesso.";
                return RedirectToAction("Preferences");
            }
            return View(preferences);
        }

        public IActionResult EditPreference(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var preference = _unitOfWork.Preference.Get(u => u.Id == id);
            if (preference == null)
                return NotFound();

            return View(preference);
        }

        [HttpPost]
        public IActionResult EditPreference(Preferences preferences)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Preference.Update(preferences);
                _unitOfWork.Save();
                TempData["success"] = "Preferência atualizada com sucesso.";
                return RedirectToAction("Preferences");
            }

            return View(preferences);
        }

        public IActionResult DeletePreference(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var preference = _unitOfWork.Preference.Get(u => u.Id == id);
            if (preference == null)
                return NotFound();

            return View(preference);
        }

        [HttpPost, ActionName("DeletePreference")]
        public IActionResult DeletePreferencePost(int? id)
        {
            var preference = _unitOfWork.Preference.Get(u => u.Id == id);
            if (preference == null)
                return NotFound();

            _unitOfWork.Preference.Remove(preference);
            _unitOfWork.Save();
            TempData["success"] = "Preferência eliminada com sucesso.";
            return RedirectToAction("Preferences");
        }


        #region AJAX API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var atletesList = _unitOfWork.Atlete.GetAll(includeProperties: "Gender")
                                                 .Select(u => new
                                                 {
                                                     id = u.Id,
                                                     fullname = u.FullName,
                                                     year = u.Year,
                                                     inscriptiondate = u.InscriptionDate,
                                                     phonenumber = u.PhoneNumber,
                                                     email = u.Email,
                                                 })
                                                 .ToList();
            return Json(new { data = atletesList });
        }

        [HttpGet]
        public IActionResult Get(int? id)
        {
            var atlete = _unitOfWork.Atlete.Get(u => u.Id == id, includeProperties: "Gender");

            return View(atlete);
        }

        [HttpGet]
        public IActionResult GetAllPreferences()
        {
            var preferences = _unitOfWork.Preference.GetAll()
                .Select(p => new
                {
                    id = p.Id,
                    name = p.Name
                }).ToList();

            return Json(new { data = preferences });
        }


        #endregion
    }
}
