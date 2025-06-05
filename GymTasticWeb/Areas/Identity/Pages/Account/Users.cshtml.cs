using GymTastic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class UsersModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public UsersModel(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public List<UserWithRoleViewModel> Users { get; set; } = new();

    public async Task OnGetAsync()
    {
        var allUsers = _userManager.Users.ToList();
        var userWithRoles = new List<UserWithRoleViewModel>();

        foreach (var user in allUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userWithRoles.Add(new UserWithRoleViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Role = roles.FirstOrDefault() ?? "Sem Role"
            });
        }

        Users = userWithRoles;
    }
}
