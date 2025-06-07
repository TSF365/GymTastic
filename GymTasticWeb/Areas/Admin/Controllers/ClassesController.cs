using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;
using GymTastic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace GymTasticWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
            classesTrainerViewModel.SpecialityList = _unitOfWork.Speciality.GetAll().Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
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
            classesTrainerViewModel.SpecialityList = _unitOfWork.Speciality.GetAll().Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
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
            classesTrainerViewModel.SpecialityList = _unitOfWork.Speciality.GetAll().Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            });

            return View(classesTrainerViewModel);
        }

        [HttpPost]
        public IActionResult Edit(ClassesTrainerViewModel classesTrainerViewModel)
        {
            if (ModelState.IsValid)
            {
                // Buscar a aula original no banco de dados
                var originalClass = _unitOfWork.Classes.Get(c => c.Id == classesTrainerViewModel.Classes.Id);
                if (originalClass == null)
                {
                    return NotFound();
                }

                // Atualizar apenas os campos permitidos (NÃO alterar RegAtletes)
                originalClass.ClassName = classesTrainerViewModel.Classes.ClassName;
                originalClass.ClassTime = classesTrainerViewModel.Classes.ClassTime;
                originalClass.TrainerId = classesTrainerViewModel.Classes.TrainerId;
                originalClass.SpecialityId = classesTrainerViewModel.Classes.SpecialityId;
                originalClass.MaxAtletes = classesTrainerViewModel.Classes.MaxAtletes;

                // RegAtletes permanece intacto

                _unitOfWork.Classes.Update(originalClass);
                _unitOfWork.Save();
                TempData["success"] = "Aula atualizada com sucesso.";

                return RedirectToAction("Index", "Classes");
            }

            // Recarregar listas se ModelState for inválido
            classesTrainerViewModel.TrainerList = _unitOfWork.Trainer.GetAll().Select(u => new SelectListItem
            {
                Text = u.FullName,
                Value = u.Id.ToString()
            });

            classesTrainerViewModel.SpecialityList = _unitOfWork.Speciality.GetAll().Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            });

            return View(classesTrainerViewModel);
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
        //public IActionResult Feedbacks()
        //{
        //    var feedbacks = _unitOfWork.ClassFeedback
        //        .GetAll(includeProperties: "Class,Atlete")
        //        .OrderByDescending(f => f.FeedbackDate)
        //        .ToList();

        //    return View(feedbacks);
        //}

        public IActionResult Feedbacks()
        {
            return View(); 
        }


        #region AJAX API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var classesList = _unitOfWork.Classes.GetAll(includeProperties: "Trainer").ToList();
            var specialities = _unitOfWork.Speciality.GetAll().ToList();

            var result = classesList.Select(cls => new
            {
                id = cls.Id,
                classname = cls.ClassName,
                classtime = cls.ClassTime,
                trainerid = cls.TrainerId,
                email = cls.Trainer?.Email,
                maxatletes = cls.MaxAtletes,
                regatletes = cls.RegAtletes,
                // Novo campo: especialidades do treinador
                speciality = cls.Speciality != null ? cls.Speciality.Name : "N/A"
            });

            return Json(new { data = result });
        }

        

        [HttpGet]
        public IActionResult Get(int? id)
        {
            var classes = _unitOfWork.Classes.Get(u => u.Id == id, includeProperties: "Trainer");

            return View(classes);
        }

        [HttpGet]
        public IActionResult GetAllFeedbacks()
        {
            var feedbacks = _unitOfWork.ClassFeedback
                .GetAll(includeProperties: "Class,Atlete")
                .OrderByDescending(f => f.FeedbackDate)
                .Select(f => new
                {
                    className = f.Class?.ClassName,
                    atleteName = f.Atlete?.FullName,
                    comment = f.Comment,
                    rating = f.Rating,
                    date = f.FeedbackDate.ToString("dd/MM/yyyy")
                }).ToList();

            return Json(new { data = feedbacks });
        }

        #endregion
    }
}
