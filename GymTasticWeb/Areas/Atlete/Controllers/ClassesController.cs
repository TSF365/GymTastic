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
                ClassFeedback = new ClassFeedback()
            };

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var atlete = _unitOfWork.Atlete.Get(a => a.UserId == userId);
            if (atlete == null)
            {
                return NotFound("Atleta não encontrado.");
            }

            feedbackClassViewModel.ClassFeedback.AtleteId = atlete.Id;

            // IDs das aulas em que o atleta está inscrito
            var registeredClassIds = _unitOfWork.ClassRegistration
                .GetAll()
                .Where(r => r.AtleteId == atlete.Id)
                .Select(r => r.ClassId)
                .ToList();

            // Pega todas as aulas e filtra em memória
            var eligibleClasses = _unitOfWork.Classes
                .GetAll()
                .Where(c => registeredClassIds.Contains(c.Id) && c.ClassTime < DateTime.Now)
                .ToList();

            feedbackClassViewModel.ClassesList = eligibleClasses.Select(c => new SelectListItem
            {
                Text = c.ClassName,
                Value = c.Id.ToString()
            });

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(int classId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var atlete = _unitOfWork.Atlete.Get(a => a.UserId == userId);

            if (atlete == null)
            {
                return NotFound("Atleta não encontrado.");
            }

            var selectedClass = _unitOfWork.Classes.Get(c => c.Id == classId);

            if (selectedClass == null)
            {
                return NotFound("Aula não encontrada.");
            }

            if (selectedClass.RegAtletes >= selectedClass.MaxAtletes)
            {
                TempData["Error"] = "A aula está cheia. Por favor, escolha outra aula.";
                return RedirectToAction(nameof(Index));
            }

            var alreadyRegistered = _unitOfWork.ClassRegistration.Get(
                r => r.ClassId == classId && r.AtleteId == atlete.Id);

            if (alreadyRegistered != null)
            {
                TempData["Error"] = "Você já está inscrito nesta aula.";
                return RedirectToAction(nameof(Index));
            }

            var registration = new ClassRegistration
            {
                ClassId = classId,
                AtleteId = atlete.Id
            };

            selectedClass.RegAtletes += 1;

            _unitOfWork.ClassRegistration.Add(registration);
            _unitOfWork.Classes.Update(selectedClass);
            _unitOfWork.Save();

            TempData["Success"] = "Inscrição realizada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Unregister(int classId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var atlete = _unitOfWork.Atlete.Get(a => a.UserId == userId);

            if (atlete == null)
            {
                return NotFound("Atleta não encontrado.");
            }

            var registration = _unitOfWork.ClassRegistration.Get(
                r => r.ClassId == classId && r.AtleteId == atlete.Id);

            if (registration == null)
            {
                TempData["Error"] = "Você não está inscrito nesta aula.";
                return RedirectToAction(nameof(Index));
            }

            var selectedClass = _unitOfWork.Classes.Get(c => c.Id == classId);
            if (selectedClass != null)
            {
                if (selectedClass.RegAtletes > 0)
                {
                    selectedClass.RegAtletes -= 1;
                    _unitOfWork.Classes.Update(selectedClass);
                }
            }

            _unitOfWork.ClassRegistration.Remove(registration);
            _unitOfWork.Save();

            TempData["Success"] = "Inscrição cancelada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterSugestion(int classId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var atlete = _unitOfWork.Atlete.Get(a => a.UserId == userId);

            if (atlete == null)
            {
                return NotFound("Atleta não encontrado.");
            }

            var selectedClass = _unitOfWork.Classes.Get(c => c.Id == classId);

            if (selectedClass == null)
            {
                return NotFound("Aula não encontrada.");
            }

            if (selectedClass.RegAtletes >= selectedClass.MaxAtletes)
            {
                TempData["Error"] = "A aula está cheia. Por favor, escolha outra aula.";
                return RedirectToAction("Index", "Home");
            }

            var alreadyRegistered = _unitOfWork.ClassRegistration.Get(
                r => r.ClassId == classId && r.AtleteId == atlete.Id);

            if (alreadyRegistered != null)
            {
                TempData["Error"] = "Você já está inscrito nesta aula.";
                return RedirectToAction("Index", "Home");
            }

            var registration = new ClassRegistration
            {
                ClassId = classId,
                AtleteId = atlete.Id
            };

            selectedClass.RegAtletes += 1;

            _unitOfWork.ClassRegistration.Add(registration);
            _unitOfWork.Classes.Update(selectedClass);
            _unitOfWork.Save();

            TempData["Success"] = "Inscrição realizada com sucesso!";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UnregisterSugestion(int classId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var atlete = _unitOfWork.Atlete.Get(a => a.UserId == userId);

            if (atlete == null)
            {
                return NotFound("Atleta não encontrado.");
            }

            var registration = _unitOfWork.ClassRegistration.Get(
                r => r.ClassId == classId && r.AtleteId == atlete.Id);

            if (registration == null)
            {
                TempData["Error"] = "Você não está inscrito nesta aula.";
                return RedirectToAction("Index", "Home");
            }

            var selectedClass = _unitOfWork.Classes.Get(c => c.Id == classId);
            if (selectedClass != null)
            {
                if (selectedClass.RegAtletes > 0)
                {
                    selectedClass.RegAtletes -= 1;
                    _unitOfWork.Classes.Update(selectedClass);
                }
            }

            _unitOfWork.ClassRegistration.Remove(registration);
            _unitOfWork.Save();

            TempData["Success"] = "Inscrição cancelada com sucesso!";
            return RedirectToAction("Index", "Home");
        }


        #region AJAX API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { data = new List<object>() }); // ou retorna erro
            }

            var atlete = _unitOfWork.Atlete.Get(a => a.UserId == userId);

            if (atlete == null)
            {
                return Json(new { data = new List<object>() }); // ou retorna erro
            }

            // Pega todos os IDs das aulas que o atleta já está inscrito
            var registrations = _unitOfWork.ClassRegistration
                .GetAll()
                .Where(r => r.AtleteId == atlete.Id)
                .Select(r => r.ClassId)
                .ToHashSet();

            // Pega a lista completa das aulas com dados relacionados (Trainer e Speciality)
            var classesList = _unitOfWork.Classes.GetAll(includeProperties: "Trainer,Speciality").ToList();

            // Projeta os dados para o formato que o DataTables espera
            var result = classesList.Select(cls => new
            {
                id = cls.Id,
                classname = cls.ClassName,
                classtime = cls.ClassTime,
                trainerid = cls.TrainerId,
                email = cls.Trainer?.Email,
                maxatletes = cls.MaxAtletes,
                regatletes = cls.RegAtletes,
                speciality = cls.Speciality != null ? cls.Speciality.Name : "N/A",
                isRegistered = registrations.Contains(cls.Id)
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
        public IActionResult GetAllSuggestions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Json(new { data = new List<object>() });

            // Buscar o atleta pelo user ID
            var atlete = _unitOfWork.Atlete.Get(a => a.UserId == userId);
            if (atlete == null)
                return Json(new { data = new List<object>() });

            // Buscar IDs de preferências associadas ao atleta
            var preferenceIds = _unitOfWork.AtletePreference
                .GetAll()
                .Where(ap => ap.Id_Atlete == atlete.Id)
                .Select(ap => ap.Id_Preference)
                .ToList();

            if (!preferenceIds.Any())
                return Json(new { data = new List<object>() });

            // Buscar nomes das preferências
            var preferenceNames = _unitOfWork.Preference
                .GetAll()
                .Where(p => preferenceIds.Contains(p.Id))
                .Select(p => p.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Buscar todas as aulas
            var allClasses = _unitOfWork.Classes.GetAll(includeProperties: "Trainer,Speciality").ToList();

            // Filtrar aulas que têm especialidade com nome nas preferências
            var filteredClasses = allClasses
                .Where(cls => cls.Speciality != null && preferenceNames.Contains(cls.Speciality.Name))
                .ToList();

            // Buscar aulas em que o atleta já está inscrito
            var registeredClassIds = _unitOfWork.ClassRegistration
                .GetAll()
                .Where(r => r.AtleteId == atlete.Id)
                .Select(r => r.ClassId)
                .ToHashSet();

            // Projeção para DataTables
            var result = filteredClasses.Select(cls => new
            {
                id = cls.Id,
                classname = cls.ClassName,
                classtime = cls.ClassTime,
                trainerid = cls.TrainerId,
                email = cls.Trainer?.Email,
                maxatletes = cls.MaxAtletes,
                regatletes = cls.RegAtletes,
                speciality = cls.Speciality?.Name ?? "N/A",
                isRegistered = registeredClassIds.Contains(cls.Id)
            });

            return Json(new { data = result });
        }


        #endregion
    }
}
