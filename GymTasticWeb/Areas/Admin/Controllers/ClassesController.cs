using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;
using GymTastic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace GymTasticWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        public IActionResult Create()
        {
            ClassesTrainerViewModel classesTrainerViewModel = new ClassesTrainerViewModel();
            classesTrainerViewModel.Classes = new Classes();
            classesTrainerViewModel.TrainerList = _unitOfWork.Trainer.GetAll().Select(u => new SelectListItem
            {
                Text = u.FullName,
                Value = u.Id.ToString()
            });

            return View(classesTrainerViewModel);
        }


        [HttpPost]
        public IActionResult Create(ClassesTrainerViewModel classesTrainerViewModel)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Classes.Add(classesTrainerViewModel.Classes);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            classesTrainerViewModel.TrainerList = _unitOfWork.Trainer.GetAll().Select(u => new SelectListItem
            {
                Text = u.FullName,
                Value = u.Id.ToString()
            });

            return View(classesTrainerViewModel);
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
