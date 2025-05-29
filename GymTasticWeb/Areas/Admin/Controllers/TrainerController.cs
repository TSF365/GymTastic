using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;
using GymTastic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymTasticWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TrainerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public TrainerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var trainersList = _unitOfWork.Trainer.GetAll().ToList();
            return View(trainersList);
        }

        //public IActionResult Create()
        //{
        //    GymTastic.Models.Models.Trainer trainer = new GymTastic.Models.Models.Trainer();

        //    return View(trainer);
        //}

        //[HttpPost]
        //public IActionResult Create(GymTastic.Models.Models.Trainer trainer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Trainer.Add(trainer);
        //        _unitOfWork.Save();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(trainer);
        //}

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            GymTastic.Models.Models.Trainer? trainerResult = _unitOfWork.Trainer.Get(i => i.Id == id);
            if (trainerResult == null)
            {
                return NotFound();
            }

            GymTastic.Models.Models.Trainer trainer = new GymTastic.Models.Models.Trainer();
            trainer = trainerResult;


            return View(trainer);
        }

        [HttpPost]
        public IActionResult Edit(GymTastic.Models.Models.Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Trainer.Update(trainer);
                _unitOfWork.Save();
                TempData["success"] = "Atleta atualizado com sucesso.";

                return RedirectToAction("Index", "Trainer");

            }
            return View();

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var trainerResult = _unitOfWork.Trainer.Get(u => u.Id == id);
            if (trainerResult == null)
            {
                return NotFound();
            }
            return View(trainerResult);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var trainerResult = _unitOfWork.Trainer.Get(u => u.Id == id);
            if (trainerResult == null)
            {
                return NotFound();
            }
            _unitOfWork.Trainer.Remove(trainerResult);
            _unitOfWork.Save();
            TempData["success"] = "Treinador apagado com sucesso.";
            return RedirectToAction("Index", "Trainer");

        }

        #region AJAX API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var trainersList = _unitOfWork.Trainer.GetAll()
                                                 .Select(u => new
                                                 {
                                                     id = u.Id,
                                                     fullname = u.FullName,
                                                     speciality = u.Specialty,
                                                     tptd = u.TPTD,
                                                     tptddate = u.TptdDate,
                                                     email = u.Email,
                                                     phonenumber = u.PhoneNumber,
                                                 })
                                                 .ToList();
            return Json(new { data = trainersList });
        }

        [HttpGet]
        public IActionResult Get(int? id)
        {
            var atlete = _unitOfWork.Trainer.Get(u => u.Id == id);

            return View(atlete);
        }

        #endregion
    }
}
