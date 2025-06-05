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
                return NotFound();

            var trainer = _unitOfWork.Trainer.Get(i => i.Id == id);
            if (trainer == null)
                return NotFound();

            var allSpecialities = _unitOfWork.Speciality.GetAll().ToList();
            var selectedSpecialityIds = _unitOfWork.TrainerSpeciality
                .GetAll().Where(ts => ts.Id_Trainer == trainer.Id)
                .Select(ts => ts.Id_Speciality).ToList();

            var viewModel = new TrainerEditViewModel
            {
                Trainer = trainer,
                Specialities = allSpecialities,
                SelectedSpecialityIds = selectedSpecialityIds
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Edit(TrainerEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Reload all specialities if form validation fails
                viewModel.Specialities = _unitOfWork.Speciality.GetAll().ToList();
                return View(viewModel);
            }

            // Update trainer
            _unitOfWork.Trainer.Update(viewModel.Trainer);

            // Remove existing specialities
            var existingSpecialities = _unitOfWork.TrainerSpeciality
                .GetAll().Where(ts => ts.Id_Trainer == viewModel.Trainer.Id).ToList();

            foreach (var item in existingSpecialities)
            {
                _unitOfWork.TrainerSpeciality.Remove(item);
            }

            // Add new ones
            if (viewModel.SelectedSpecialityIds != null)
            {
                foreach (var specialityId in viewModel.SelectedSpecialityIds)
                {
                    _unitOfWork.TrainerSpeciality.Add(new TrainerSpeciality
                    {
                        Id_Trainer = viewModel.Trainer.Id,
                        Id_Speciality = specialityId
                    });
                }
            }

            _unitOfWork.Save();
            TempData["success"] = "Treinador atualizado com sucesso.";
            return RedirectToAction("Index");
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
            var trainer = _unitOfWork.Trainer.Get(u => u.Id == id);
            if (trainer == null)
                return NotFound();

            var specialities = _unitOfWork.TrainerSpeciality.GetAll()
                .Where(ts => ts.Id_Trainer == id).ToList();

            foreach (var s in specialities)
            {
                _unitOfWork.TrainerSpeciality.Remove(s);
            }

            _unitOfWork.Trainer.Remove(trainer);
            _unitOfWork.Save();

            TempData["success"] = "Treinador apagado com sucesso.";
            return RedirectToAction("Index");
        }

        public IActionResult CreateSpeciality()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateSpeciality(Speciality speciality)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Speciality.Add(speciality);
                _unitOfWork.Save();
                TempData["success"] = "Especialidade criada com sucesso.";
                return RedirectToAction("Specialities");
            }

            return View(speciality);
        }

        public IActionResult EditSpeciality(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var speciality = _unitOfWork.Speciality.Get(u => u.Id == id);
            if (speciality == null)
                return NotFound();

            return View(speciality);
        }

        [HttpPost]
        public IActionResult EditSpeciality(Speciality speciality)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Speciality.Update(speciality);
                _unitOfWork.Save();
                TempData["success"] = "Especialidade atualizada com sucesso.";
                return RedirectToAction("Specialities");
            }

            return View(speciality);
        }

        public IActionResult DeleteSpeciality(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var speciality = _unitOfWork.Speciality.Get(u => u.Id == id);
            if (speciality == null)
                return NotFound();

            return View(speciality);
        }

        [HttpPost]
        public IActionResult DeleteSpecialityPost(int? id)
        {
            var speciality = _unitOfWork.Speciality.Get(u => u.Id == id);
            if (speciality == null)
                return NotFound();

            _unitOfWork.Speciality.Remove(speciality);
            _unitOfWork.Save();
            TempData["success"] = "Especialidade apagada com sucesso.";
            return RedirectToAction("Specialities");
        }

        public IActionResult Specialities()
        {
            return View();
        }


        #region AJAX API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var trainers = _unitOfWork.Trainer.GetAll().ToList();
            var trainerSpecialities = _unitOfWork.TrainerSpeciality.GetAll().ToList();
            var specialities = _unitOfWork.Speciality.GetAll().ToList();

            var trainerList = trainers.Select(trainer => new
            {
                id = trainer.Id,
                fullname = trainer.FullName,
                phonenumber = trainer.PhoneNumber,
                email = trainer.Email,
                tptd = trainer.TPTD,
                tptddate = trainer.TptdDate,

                // Retornar como lista de strings
                speciality = trainerSpecialities
                                .Where(ts => ts.Id_Trainer == trainer.Id)
                                .Join(specialities,
                                      ts => ts.Id_Speciality,
                                      s => s.Id,
                                      (ts, s) => s.Name)
                                .ToList()
            }).ToList();

            return Json(new { data = trainerList });
        }



        [HttpGet]
        public IActionResult Get(int? id)
        {
            var atlete = _unitOfWork.Trainer.Get(u => u.Id == id);

            return View(atlete);
        }

        [HttpGet]
        public IActionResult GetAllSpecialities()
        {
            var specialities = _unitOfWork.Speciality.GetAll()
                .Select(s => new
                {
                    id = s.Id,
                    name = s.Name
                }).ToList();

            return Json(new { data = specialities });
        }

        #endregion
    }
}
