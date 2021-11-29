using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MilkyPantsCheese.Pages
{
	/// <summary>
	/// Modelo de la pagina de cerrar sesion
	/// </summary>
	[Authorize]
    public class LogoutModel : PageModel
    {
	    public readonly MilkySignInManager _signInManager;

	    public LogoutModel(MilkySignInManager signInManager)
	    {
		    _signInManager = signInManager;
	    }

        public async Task OnGet()
        {
			if (User.Identity.IsAuthenticated)
			{
				await _signInManager.SignOutAsync();
			}
		}
    }
}
