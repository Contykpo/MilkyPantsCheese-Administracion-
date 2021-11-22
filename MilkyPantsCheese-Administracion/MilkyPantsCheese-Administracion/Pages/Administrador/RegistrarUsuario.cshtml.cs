using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
    [Authorize(Roles = Constantes.NombreRolAdministrador)]
    public class RegistrarUsuarioModel : PageModel
    {
	    public readonly MilkyUserManager _userManager;
	    public readonly MilkyDbContext _dbContext;

	    public readonly ILogger<RegistrarUsuarioModel> _logger;

        public RegistrarUsuarioModel(MilkyUserManager userManager, MilkyDbContext dbContext, ILogger<RegistrarUsuarioModel> logger)
        {
	        _userManager = userManager;
	        _dbContext = dbContext;
	        _logger = logger;
        }
        
        [BindProperty]
        [PasswordPropertyText]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        public string Contrase�a { get; set; }

        [BindProperty]
        [PasswordPropertyText]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Compare(nameof(Contrase�a), ErrorMessage = "Las contrase�as deben ser iguales")]
        [DisplayName("Confirmacion contrase�a")]
        public string ConfirmacionContrase�a { get; set; }

        [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [DisplayName("Nombre del usuario")]
        public string NombreUsuario { get; set; }

        [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [DisplayName("Tipo de usuario")]
        public string TipoDeUsuarioSeleccionado { get; set; }

        [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [DisplayName("Duracion de la sesion (en horas)")]
        public TimeSpan DuracionSesion { get; set; } = TimeSpan.FromHours(1);

        public List<SelectListItem> TiposUsuarioDisponibles { get; set; } = UserHelpers.ObtenerTiposDeUsuarioDisponibles();

        public async Task<IActionResult> OnPost()
        {
	        if (!ModelState.IsValid)
		        return Page();

	        var nuevoUsuario = new ModeloUsuario
	        {
		        UserName = NombreUsuario,
		        DuracionSesion = DuracionSesion,
	        };

            //Nos aseguramos de que la contrase�a cumpla con los requisitos de seguridad
	        foreach (var passwordValidator in _userManager.PasswordValidators)
	        {
		        if (passwordValidator.ValidateAsync(_userManager, nuevoUsuario, Contrase�a).Result != IdentityResult.Success)
		        {
                    ModelState.AddModelError(nameof(Contrase�a), "La contrase�a no cumple con los requisitos de seguridad");

                    return Page();
		        }
	        }

	        nuevoUsuario.PasswordHash = _userManager.PasswordHasher.HashPassword(nuevoUsuario, Contrase�a);

	        try
	        {
                //Creamos el nuevo usuario
		        await _userManager.CreateAsync(nuevoUsuario);

		        //A�adimos el nuevo usuario a su rol correspondiente
		        await _userManager.AddToRoleAsync(nuevoUsuario, TipoDeUsuarioSeleccionado);

                //Guardamos los cambios
                await _dbContext.SaveChangesAsync();
	        }
	        catch (Exception ex)
	        {
                _logger.Log(LogLevel.Trace, ex, "Error al crear usuario");

		        return RedirectToPage("/Error");
	        }

	        return RedirectToPage("/Index");
        }
    }
}
