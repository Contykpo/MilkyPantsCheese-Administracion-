using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MilkyPantsCheese.Pages
{
    public class LoginModel : PageModel
    {
	    public readonly MilkyUserManager _userManager;
	    public readonly MilkySignInManager _signInManager;
	    public readonly MilkyDbContext _dbContext;

	    public LoginModel(MilkyUserManager userManager, MilkySignInManager signInManager, MilkyDbContext dbContext)
	    {
		    _userManager = userManager;
		    _signInManager = signInManager;
		    _dbContext = dbContext;
	    }

        [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        public string Nombre { get; set; }

        [BindProperty]
        [PasswordPropertyText(true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        public string Contraseña { get; set; }

        public async Task<IActionResult> OnPost()
        {
	        if (!ModelState.IsValid)
		        return Page();

			//Intentamos encontrar el usuario con el nombre especificado
	        var usuarioEncontrado = await _dbContext.Users.FirstOrDefaultAsync(u => Nombre == u.UserName);

			//Si no se pudo encontrar un usuario con ese nombre añadirmos un mensaje de error y recargamos la pagina
	        if (usuarioEncontrado == null)
	        {
		        ModelState.AddModelError(nameof(Nombre), "Usuario incorrecto");

		        return Page();
	        }

			//Si pudimos encontrar el usuario entonces procedemos a comprobar la contraseña. Si la comprobacion falla, añadimos un mensaje de error y recargamos la pagina
	        if (_userManager.PasswordHasher.VerifyHashedPassword(usuarioEncontrado, usuarioEncontrado.PasswordHash, Contraseña) != PasswordVerificationResult.Success)
	        {
				ModelState.AddModelError(nameof(Contraseña), "Contraseña incorrecta");

				return Page();
	        }

			//Si llegamos hasta este punto significa que comprobamos los datos ingresados asi que iniciamos sesion
	        await _signInManager.SignInAsync(usuarioEncontrado, new AuthenticationProperties
	        {
		        ExpiresUtc = DateTimeOffset.UtcNow.Add(usuarioEncontrado.DuracionSesion),
		        IssuedUtc = DateTimeOffset.UtcNow
	        });

	        return RedirectToPage("/Index");
        }
    }
}
