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
        public string Contraseña { get; set; }

        [BindProperty]
        [PasswordPropertyText]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Compare(nameof(Contraseña), ErrorMessage = "Las contraseñas deben ser iguales")]
        [DisplayName("Confirmacion contraseña")]
        public string ConfirmacionContraseña { get; set; }

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

            //Nos aseguramos de que la contraseña cumpla con los requisitos de seguridad
	        foreach (var passwordValidator in _userManager.PasswordValidators)
	        {
		        if (passwordValidator.ValidateAsync(_userManager, nuevoUsuario, Contraseña).Result != IdentityResult.Success)
		        {
                    ModelState.AddModelError(nameof(Contraseña), "La contraseña no cumple con los requisitos de seguridad");

                    return Page();
		        }
	        }

	        nuevoUsuario.PasswordHash = _userManager.PasswordHasher.HashPassword(nuevoUsuario, Contraseña);

	        try
	        {
                //Creamos el nuevo usuario
		        await _userManager.CreateAsync(nuevoUsuario);

		        //Añadimos el nuevo usuario a su rol correspondiente
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
