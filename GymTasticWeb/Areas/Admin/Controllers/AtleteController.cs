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

            return View(atleteViewModel);
        }

        [HttpPost]
        public IActionResult Create(AtleteViewModel atleteViewModel)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Atlete.Add(atleteViewModel.Atlete);
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

            return View(atleteViewModel);
        }

        [HttpPost]
        public IActionResult Edit(AtleteViewModel atleteViewModel, IFormFile? file)
        {
            string oldFileNameWithPath = string.Empty;
            if (ModelState.IsValid)
            {
                _unitOfWork.Atlete.Update(atleteViewModel.Atlete);
                _unitOfWork.Save();
                TempData["success"] = "Atleta atualizado com sucesso.";

                return RedirectToAction("Index", "Atlete");

            }
            return View();

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

        #endregion
    }
}
