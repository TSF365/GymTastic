using GymTastic.DataAccess.Data;
using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;
using GymTastic.Models.ViewModels;
using GymTasticWeb.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace GymTasticWeb.Areas.Identity.Pages.Account
{
    public class RegisterTrainerModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterTrainerModel(UserManager<IdentityUser> userManager,
                                   SignInManager<IdentityUser> signInManager,
                                   ApplicationDbContext context,
                                   RoleManager<IdentityRole> roleManager,
                                   IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public RegisterTrainerViewModel Input { get; set; }

        public void OnGet()
        {
            Input = new RegisterTrainerViewModel
            {
                SpecialityList = _unitOfWork.Speciality.GetAll()
                    .Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() })
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Input.SpecialityList = _unitOfWork.Speciality.GetAll()
                .Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() });

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new IdentityUser
            {
                UserName = Input.Email,
                Email = Input.Email
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Trainer"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Trainer"));
                }

                await _userManager.AddToRoleAsync(user, "Trainer");

                Input.Trainer.UserId = user.Id;
                _unitOfWork.Trainer.Add(Input.Trainer);
                _unitOfWork.Save();

                // Add trainer-speciality relationships
                foreach (var specId in Input.SelectedSpecialityIds)
                {
                    var trainerSpeciality = new TrainerSpeciality
                    {
                        Id_Trainer = Input.Trainer.Id,
                        Id_Speciality = specId
                    };
                    _unitOfWork.TrainerSpeciality.Add(trainerSpeciality);
                }
                _unitOfWork.Save();

                return RedirectToPage("/Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return Page();
                                                                            }
    }
}
