using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkyPantsCheese_Administracion.Modelos;

namespace MilkyPantsCheese.Pages
{
	[Authorize]
    public class LogoutModel : PageModel
    {
	    public readonly SignInManager<ModeloUsuario> _signInManager;

	    public LogoutModel(SignInManager<ModeloUsuario> signInManager)
	    {
		    _signInManager = signInManager;
	    }

        public async Task OnGet()
        {
	        
        }

        public async Task<IActionResult> OnPost()
        {
	        if (User.Identity.IsAuthenticated)
	        {
		        await _signInManager.SignOutAsync();
	        }

	        return RedirectToPage("/Index");
        }
    }
}
