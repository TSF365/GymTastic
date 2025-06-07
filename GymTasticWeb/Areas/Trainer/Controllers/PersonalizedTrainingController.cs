using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GymTastic.Models.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace GymTasticWeb.Areas.Trainer.Controllers
{
    [Area("Trainer")]
    [Authorize(Roles = "Trainer")]
    public class PersonalizedTrainingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PersonalizedTrainingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var trainingList = _unitOfWork.PersonalizedTraining.GetAll(includeProperties: "Trainer,Atlete").ToList();
            return View(trainingList);
        }

        public IActionResult Create()
        {
            PTrainingTrainerAtleteViewModel pTrainingTrainerAtlete = new PTrainingTrainerAtleteViewModel();
            pTrainingTrainerAtlete.PersonalizedTraining = new PersonalizedTraining();
            pTrainingTrainerAtlete.TrainerList = _unitOfWork.Trainer.GetAll().Select(u => new SelectListItem
            {
                Text = u.FullName,
                Value = u.Id.ToString()
            });
            pTrainingTrainerAtlete.AtleteList = _unitOfWork.Atlete.GetAll().Select(u => new SelectListItem
            {
                Text = u.FullName,
                Value = u.Id.ToString()
            });

            return View(pTrainingTrainerAtlete);
        }

        [HttpPost]
        public IActionResult Create(PTrainingTrainerAtleteViewModel pTrainingTrainerAtlete)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.PersonalizedTraining.Add(pTrainingTrainerAtlete.PersonalizedTraining);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            pTrainingTrainerAtlete.TrainerList = _unitOfWork.Trainer.GetAll().Select(u => new SelectListItem
            {
                Text = u.FullName,
                Value = u.Id.ToString()
            });
            pTrainingTrainerAtlete.AtleteList = _unitOfWork.Atlete.GetAll().Select(u => new SelectListItem
            {
                Text = u.FullName,
                Value = u.Id.ToString()
            });

            return View(pTrainingTrainerAtlete);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            PersonalizedTraining personalizedTrainingResult = _unitOfWork.PersonalizedTraining.Get(i => i.Id == id);
            if (personalizedTrainingResult == null)
            {
                return NotFound();
            }

            PTrainingTrainerAtleteViewModel pTrainingTrainerAtlete = new PTrainingTrainerAtleteViewModel();
            pTrainingTrainerAtlete.PersonalizedTraining = personalizedTrainingResult;

            pTrainingTrainerAtlete.TrainerList = _unitOfWork.Trainer.GetAll().Select(u => new SelectListItem
            {
                Text = u.FullName,
                Value = u.Id.ToString()
            });
            pTrainingTrainerAtlete.AtleteList = _unitOfWork.Atlete.GetAll().Select(u => new SelectListItem
            {
                Text = u.FullName,
                Value = u.Id.ToString()
            });

            return View(pTrainingTrainerAtlete);
        }

        [HttpPost]
        public IActionResult Edit(PTrainingTrainerAtleteViewModel pTrainingTrainerAtlete)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.PersonalizedTraining.Add(pTrainingTrainerAtlete.PersonalizedTraining);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            pTrainingTrainerAtlete.TrainerList = _unitOfWork.Trainer.GetAll().Select(u => new SelectListItem
            {
                Text = u.FullName,
                Value = u.Id.ToString()
            });
            pTrainingTrainerAtlete.AtleteList = _unitOfWork.Atlete.GetAll().Select(u => new SelectListItem
            {
                Text = u.FullName,
                Value = u.Id.ToString()
            });

            return View(pTrainingTrainerAtlete);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var trainingResult = _unitOfWork.PersonalizedTraining.Get(u => u.Id == id);
            if (trainingResult == null)
            {
                return NotFound();
            }
            return View(trainingResult);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var trainingResult = _unitOfWork.PersonalizedTraining.Get(u => u.Id == id);
            if (trainingResult == null)
            {
                return NotFound();
            }
            _unitOfWork.PersonalizedTraining.Remove(trainingResult);
            _unitOfWork.Save();
            TempData["success"] = "Treino apagado com sucesso.";
            return RedirectToAction("Index", "PersonalizedTraining");

        }

        #region AJAX API CALLS

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var trainingList = _unitOfWork.PersonalizedTraining.GetAll(includeProperties: "Trainer,Atlete")
        //                                         .Select(u => new
        //                                         {
        //                                             id = u.Id,
        //                                             trainingname = u.TrainingName,
        //                                             atleteid = u.AtleteId,
        //                                             atlete = u.Atlete.FullName,
        //                                             trainingobjective = u.TrainingObjective,
        //                                             trainerid = u.TrainerId,
        //                                             email = u.Trainer.Email,
        //                                             trainer = u.Trainer.FullName,
        //                                             //speciality = u.Trainer.Specialty,
        //                                         })
        //                                         .ToList();
        //    return Json(new { data = trainingList });
        //}

        [HttpGet]
        public IActionResult GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // ID do utilizador autenticado

            var trainer = _unitOfWork.Trainer.Get(t => t.UserId == userId);
            if (trainer == null)
            {
                return Unauthorized();
            }

            // Obter todos os treinos personalizados do treinador atual
            var trainingList = _unitOfWork.PersonalizedTraining
                .GetAll("Trainer,Atlete")
                .Where(t => t.TrainerId == trainer.Id) // filtrar na memória
                .Select(u => new
                {
                    id = u.Id,
                    trainingname = u.TrainingName,
                    atleteid = u.AtleteId,
                    atlete = u.Atlete.FullName,
                    trainingobjective = u.TrainingObjective,
                    trainerid = u.TrainerId,
                    email = u.Trainer.Email,
                    trainer = u.Trainer.FullName,
                    // speciality = u.Trainer.Specialty // descomentar se estiver implementado
                })
                .ToList();

            return Json(new { data = trainingList });
        }


        [HttpGet]
        public IActionResult Get(int? id)
        {
            var training = _unitOfWork.PersonalizedTraining.Get(u => u.Id == id, includeProperties: "Trainer,Atlete");

            return View(training);
        }

        #endregion
    }
}
