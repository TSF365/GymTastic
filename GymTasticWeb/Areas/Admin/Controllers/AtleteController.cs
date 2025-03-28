using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;
using GymTastic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymTasticWeb.Areas.Admin.Controllers
{
    public class AtleteController : Controller
    {
        private readonly IUnityOfWork _unityOfWork;

        public AtleteController(IUnityOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }

        public IActionResult Index()
        {
            var atletesList = _unityOfWork.Atlete.GetAll(includeProperties: "Gender").ToList();
            return View(atletesList);
        }

        public IActionResult Create()
        {
            AtleteViewModel atleteViewModel = new AtleteViewModel();
            atleteViewModel.Atlete = new Atlete();
            atleteViewModel.GenderList = _unityOfWork.Gender.GetAll().Select(u => new SelectListItem
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
                _unityOfWork.Atlete.Add(atleteViewModel.Atlete);
                _unityOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(atleteViewModel);
        }

        #region AJAX API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var atletesList = _unityOfWork.Atlete.GetAll(includeProperties: "Gender")
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
            var atlete = _unityOfWork.Atlete.Get(u => u.Id == id, includeProperties: "Gender");

            return View(atlete);
        }

        #endregion
    }
}
