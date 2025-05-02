using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;
using GymTastic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace GymTasticWeb.Areas.Admin.Controllers
{
    [Area("Trainer")]
    [Authorize(Roles = "Trainer")]
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

        public IActionResult Edit(int? id) 
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Classes classesResult = _unitOfWork.Classes.Get(i => i.Id == id);
            if (classesResult == null)
            {
                return NotFound();
            }

            ClassesTrainerViewModel classesTrainerViewModel = new ClassesTrainerViewModel();
            classesTrainerViewModel.Classes = classesResult;

            classesTrainerViewModel.TrainerList = _unitOfWork.Trainer.GetAll().Select(u => new SelectListItem
            {
                Text = u.FullName,
                Value = u.Id.ToString()
            });

            return View(classesTrainerViewModel);
        }

        [HttpPost]
        public IActionResult Edit(ClassesTrainerViewModel classesTrainerViewModel)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Classes.Update(classesTrainerViewModel.Classes);
                _unitOfWork.Save();
                TempData["success"] = "Aula atualizada com sucesso.";

                return RedirectToAction("Index", "Classes");

            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var classesResult = _unitOfWork.Classes.Get(u => u.Id == id);
            if (classesResult == null)
            {
                return NotFound();
            }
            return View(classesResult);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var classesResult = _unitOfWork.Classes.Get(u => u.Id == id);
            if (classesResult == null)
            {
                return NotFound();
            }
            _unitOfWork.Classes.Remove(classesResult);
            _unitOfWork.Save();
            TempData["success"] = "Aula apagada com sucesso.";
            return RedirectToAction("Index", "Classes");

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
