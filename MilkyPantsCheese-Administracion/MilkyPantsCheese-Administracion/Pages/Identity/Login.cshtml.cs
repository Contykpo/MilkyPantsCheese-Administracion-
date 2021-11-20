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
using MilkyPantsCheese_Administracion.Modelos;

namespace MilkyPantsCheese.Pages
{
    public class LoginModel : PageModel
    {
	    public readonly UserManager<ModeloUsuario> _userManager;
	    public readonly SignInManager<ModeloUsuario> _signInManager;
	    public readonly MilkyDbContext _dbContext;

	    public LoginModel(UserManager<ModeloUsuario> userManager, SignInManager<ModeloUsuario> signInManager, MilkyDbContext dbContext)
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

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
	        if (!ModelState.IsValid)
		        return Page();

	        var usuarioEncontrado = await _dbContext.Users.FirstOrDefaultAsync(u => string.Equals(Nombre, Nombre, StringComparison.Ordinal));

	        if (_userManager.PasswordHasher.VerifyHashedPassword(usuarioEncontrado, usuarioEncontrado.PasswordHash, Contraseña) != PasswordVerificationResult.Success)
	        {
				ModelState.AddModelError(nameof(Contraseña), "Contraseña incorrecta");

				return Page();
	        }

	        await _signInManager.SignInAsync(usuarioEncontrado, new AuthenticationProperties
	        {
		        ExpiresUtc = DateTimeOffset.Now + TimeSpan.FromHours(1),
		        IssuedUtc = DateTimeOffset.Now
	        });

	        return RedirectToPage("/Index");
        }
    }
}
