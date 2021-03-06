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
	/// <summary>
	/// Modelo para la pagina de login
	/// </summary>
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
        public string Contraseņa { get; set; }

		/// <summary>
		/// Intenta encontrar al usuario con las credenciales ingresadas e iniciar sesion si es posible
		/// </summary>
		public async Task<IActionResult> OnPost()
        {
	        if (!ModelState.IsValid)
		        return Page();

			//Intentamos encontrar el usuario con el nombre especificado
	        var usuarioEncontrado = await _dbContext.Users.FirstOrDefaultAsync(u => Nombre == u.UserName);

			//Si no se pudo encontrar un usuario con ese nombre aņadirmos un mensaje de error y recargamos la pagina
	        if (usuarioEncontrado == null)
	        {
		        ModelState.AddModelError(nameof(Nombre), "Usuario incorrecto");

		        return Page();
	        }

			//Si pudimos encontrar el usuario entonces procedemos a comprobar la contraseņa. Si la comprobacion falla, aņadimos un mensaje de error y recargamos la pagina
	        if (_userManager.PasswordHasher.VerifyHashedPassword(usuarioEncontrado, usuarioEncontrado.PasswordHash, Contraseņa) != PasswordVerificationResult.Success)
	        {
				ModelState.AddModelError(nameof(Contraseņa), "Contraseņa incorrecta");

				return Page();
	        }

			//Si el usuario esta deshabilitado, redireccionamos al usuario a la pagina de aviso de cuenta deshabilitada
	        if (!usuarioEncontrado.EstaHabilitado)
		        return RedirectToPage("/Identity/AvisoCuentaDeshabilitada");

			//Si el usuario esta suspendido, redireccionamos al usuario a la pagina de aviso de cuenta suspendida
	        if (usuarioEncontrado.FinSuspension > DateTimeOffset.UtcNow)
		        return RedirectToPage("/Identity/AvisoCuentaSuspendida", new{id = usuarioEncontrado.Id});

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
