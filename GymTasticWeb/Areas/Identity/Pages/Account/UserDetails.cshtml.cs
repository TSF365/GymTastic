using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class UserDetailsModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserDetailsModel(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public IdentityUser UserDetail { get; set; }
    public bool IsLockedOut { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        UserDetail = await _userManager.FindByIdAsync(id);

        if (UserDetail == null)
        {
            return NotFound();
        }

        // Verifica se o usuário está bloqueado
        IsLockedOut = UserDetail.LockoutEnd.HasValue && UserDetail.LockoutEnd > DateTimeOffset.UtcNow;

        return Page();
    }

    public async Task<IActionResult> OnPostToggleLockAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Atribui corretamente os dados para a PageModel
        UserDetail = user;
        IsLockedOut = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow;

        var email = await _userManager.GetEmailAsync(user);

        // Impede bloqueio/desbloqueio do superadministrador
        if (string.Equals(email, "superadmin@gmail.com", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(string.Empty, "O utilizador Super Admin não pode ser bloqueado ou desbloqueado.");
            return Page(); // Retorna à página com o erro e dados preenchidos
        }

        // Alternar entre bloqueado e desbloqueado
        if (IsLockedOut)
        {
            user.LockoutEnd = null; // Desbloqueia o utilizador
        }
        else
        {
            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100); // Bloqueia o utilizador
        }

        await _userManager.UpdateAsync(user);

        return RedirectToPage(new { id }); // Recarrega a página com o status atualizado
    }

}
