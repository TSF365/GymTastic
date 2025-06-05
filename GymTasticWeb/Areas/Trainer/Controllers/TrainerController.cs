using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymTasticWeb.Areas.Trainer.Controllers
{
    [Area("Trainer")]
    [Authorize(Roles = "Trainer")]
    public class TrainerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Trainer/Edit
        public IActionResult Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var trainer = _unitOfWork.Trainer.Get(t => t.UserId == userId);
            if (trainer == null)
                return NotFound();

            // Fetch and pass trainer's specialties to the view
            var trainerSpecialities = _unitOfWork.TrainerSpeciality
                .GetAll()
                .Where(ts => ts.Id_Trainer == trainer.Id)
                .Select(ts => _unitOfWork.Speciality.Get(s => s.Id == ts.Id_Speciality)?.Name)
                .Where(name => name != null)
                .ToList();

            ViewBag.Specialities = trainerSpecialities;

            return View(trainer);
        }


        // POST: Trainer/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(GymTastic.Models.Models.Trainer updatedTrainer)
        {
            if (!ModelState.IsValid)
                return View(updatedTrainer);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var existingTrainer = _unitOfWork.Trainer.Get(t => t.UserId == userId);
            if (existingTrainer == null)
                return NotFound();

            // Update only allowed fields
            existingTrainer.FullName = updatedTrainer.FullName;
            existingTrainer.Email = updatedTrainer.Email;
            existingTrainer.PhoneNumber = updatedTrainer.PhoneNumber;
            existingTrainer.TPTD = updatedTrainer.TPTD;
            existingTrainer.TptdDate = updatedTrainer.TptdDate;

            _unitOfWork.Trainer.Update(existingTrainer);
            _unitOfWork.Save();

            TempData["success"] = "Perfil atualizado com sucesso.";
            return RedirectToAction("Edit");
        }
    }
}
