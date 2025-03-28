using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;
using GymTastic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymTasticWeb.Areas.Admin.Controllers
{
    public class AtleteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AtleteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var atletesList = _unitOfWork.Atlete.GetAll(includeProperties: "Gender").ToList();
            return View(atletesList);
        }

        public IActionResult Create()
        {
            AtleteViewModel atleteViewModel = new AtleteViewModel();
            atleteViewModel.Atlete = new Atlete();
            atleteViewModel.GenderList = _unitOfWork.Gender.GetAll().Select(u => new SelectListItem
            {
                Text = u.GenderDescription,
                Value = u.Id.ToString()
            });

            return View(atleteViewModel);
        }

        [HttpPost]
        public IActionResult Create(AtleteViewModel atleteViewModel)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Atlete.Add(atleteViewModel.Atlete);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(atleteViewModel);
        }

        #region AJAX API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var atletesList = _unitOfWork.Atlete.GetAll(includeProperties: "Gender")
                                                 .Select(u => new
                                                 {
                                                     id = u.Id,
                                                     fullname = u.FullName,
                                                     year = u.Year,
                                                     inscriptiondate = u.InscriptionDate,
                                                     phonenumber = u.PhoneNumber,
                                                     email = u.Email,
                                                 })
                                                 .ToList();
            return Json(new { data = atletesList });
        }

        [HttpGet]
        public IActionResult Get(int? id)
        {
            var atlete = _unitOfWork.Atlete.Get(u => u.Id == id, includeProperties: "Gender");

            return View(atlete);
        }

        #endregion
    }
}
