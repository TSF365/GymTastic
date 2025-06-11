using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;
using GymTastic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace GymTasticWeb.Areas.Atlete.Controllers
{
    [Area("Atlete")]
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

        //[HttpPost]
        //public IActionResult Edit(PTrainingTrainerAtleteViewModel pTrainingTrainerAtlete)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.PersonalizedTraining.Add(pTrainingTrainerAtlete.PersonalizedTraining);
        //        _unitOfWork.Save();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    pTrainingTrainerAtlete.TrainerList = _unitOfWork.Trainer.GetAll().Select(u => new SelectListItem
        //    {
        //        Text = u.FullName,
        //        Value = u.Id.ToString()
        //    });
        //    pTrainingTrainerAtlete.AtleteList = _unitOfWork.Atlete.GetAll().Select(u => new SelectListItem
        //    {
        //        Text = u.FullName,
        //        Value = u.Id.ToString()
        //    });

        //    return View(pTrainingTrainerAtlete);
        //}

        #region AJAX
        [HttpGet]
        public IActionResult GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // ID do utilizador autenticado

            var atlete = _unitOfWork.Atlete.Get(t => t.UserId == userId);
            if (atlete == null)
            {
                return Unauthorized();
            }

            // Obter todos os treinos personalizados do atleta atual
            var trainingList = _unitOfWork.PersonalizedTraining
                .GetAll("Trainer,Atlete")
                .Where(t => t.AtleteId == atlete.Id) // filtrar na memória
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
        #endregion
    }
}
