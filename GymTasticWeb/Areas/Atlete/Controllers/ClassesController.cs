using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;
using GymTastic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace GymTasticWeb.Areas.Atlete.Controllers
{
    [Area("Atlete")]
    [Authorize(Roles = "Atlete")]
    public class ClassesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClassesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var classesList = _unitOfWork.Classes.GetAll(includeProperties: "Trainer").ToList();
            return View(classesList);
        }

        public IActionResult ClassFeedback()
        {
            var feedbackClassViewModel = new FeedbackClassViewModel
            {
                ClassFeedback = new ClassFeedback(),
                ClassesList = _unitOfWork.Classes.GetAll().Select(c => new SelectListItem
                {
                    Text = c.ClassName,
                    Value = c.Id.ToString()
                })
            };

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var atlete = _unitOfWork.Atlete.Get(a => a.UserId == userId);
            if (atlete == null)
            {
                return NotFound("Atleta não encontrado.");
            }

            feedbackClassViewModel.ClassFeedback.AtleteId = atlete.Id;

            return View(feedbackClassViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClassFeedback(FeedbackClassViewModel feedbackClassViewModel)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var atlete = _unitOfWork.Atlete.Get(a => a.UserId == userId);

            if (atlete == null)
            {
                return NotFound("Atleta não encontrado.");
            }

            feedbackClassViewModel.ClassFeedback.AtleteId = atlete.Id;
            feedbackClassViewModel.ClassFeedback.FeedbackDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _unitOfWork.ClassFeedback.Add(feedbackClassViewModel.ClassFeedback);
                _unitOfWork.Save();

                return RedirectToAction("Index", "Home");
            }

            feedbackClassViewModel.ClassesList = _unitOfWork.Classes.GetAll().Select(c => new SelectListItem
            {
                Text = c.ClassName,
                Value = c.Id.ToString()
            });

            return View(feedbackClassViewModel);
        }


        #region AJAX API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var classesList = _unitOfWork.Classes.GetAll(includeProperties: "Trainer")
                .Select(u => new
                {
                    id = u.Id,
                    classname = u.ClassName,
                    classtime = u.ClassTime,
                    trainerid = u.TrainerId,
                    email = u.Trainer.Email,
                    speciality = u.Trainer.Specialty,
                    maxatletes = u.MaxAtletes,
                })
                .ToList();

            return Json(new { data = classesList });
        }

        [HttpGet]
        public IActionResult Get(int? id)
        {
            var classes = _unitOfWork.Classes.Get(u => u.Id == id, includeProperties: "Trainer");
            return View(classes);
        }

        #endregion
    }
}
