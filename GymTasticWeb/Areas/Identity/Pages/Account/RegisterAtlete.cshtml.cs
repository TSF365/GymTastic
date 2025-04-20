using GymTastic.DataAccess.Data;
using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;
using GymTasticWeb.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace GymTasticWeb.Areas.Identity.Pages.Account
{
    public class RegisterAtleteModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterAtleteModel(UserManager<IdentityUser> userManager,
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
        public RegisterAtleteViewModel Input { get; set; }

        public void OnGet()
        {
            Input = new RegisterAtleteViewModel
            {
                Genders = _unitOfWork.Gender.GetAll().Select(u => new SelectListItem
                {
                    Text = u.GenderDescription,
                    Value = u.Id.ToString()
                })
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Input.Genders = _unitOfWork.Gender.GetAll().Select(u => new SelectListItem
                {
                    Text = u.GenderDescription,
                    Value = u.Id.ToString()
                });
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
                if (!await _roleManager.RoleExistsAsync("Atlete"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Atlete"));
                }

                await _userManager.AddToRoleAsync(user, "Atlete");

                Input.Atlete.UserId = user.Id;

                _unitOfWork.Atlete.Add(Input.Atlete);
                _unitOfWork.Save();

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            Input.Genders = _unitOfWork.Gender.GetAll().Select(u => new SelectListItem
            {
                Text = u.GenderDescription,
                Value = u.Id.ToString()
            });

            return Page();
        }
    }
}
