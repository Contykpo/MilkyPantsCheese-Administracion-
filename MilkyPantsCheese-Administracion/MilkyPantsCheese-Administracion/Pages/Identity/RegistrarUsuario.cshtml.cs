using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MilkyPantsCheese.Pages
{
    [Authorize(Roles = Constantes.NombreRolAdministrador)]
    public class RegistrarUsuarioModel : PageModel
    {
	    public readonly UserManager<ModeloUsuario> _userManager;
	    public readonly MilkyDbContext _dbContext;

        public RegistrarUsuarioModel(UserManager<ModeloUsuario> userManager, MilkyDbContext dbContext)
        {
	        _userManager = userManager;
	        _dbContext = dbContext;
        }
        
        [BindProperty]
        [PasswordPropertyText]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        public string Contraseņa { get; set; }

        [BindProperty]
        [PasswordPropertyText]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Compare(nameof(Contraseņa), ErrorMessage = "Las contraseņas deben ser iguales")]
        [DisplayName("Confirmacion contraseņa")]
        public string ConfirmacionContraseņa { get; set; }

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

            //Nos aseguramos de que la contraseņa cumpla con los requisitos de seguridad
	        foreach (var passwordValidator in _userManager.PasswordValidators)
	        {
		        if (passwordValidator.ValidateAsync(_userManager, nuevoUsuario, Contraseņa).Result != IdentityResult.Success)
		        {
                    ModelState.AddModelError(nameof(Contraseņa), "La contraseņa no cumple con los requisitos de seguridad");

                    return Page();
		        }
	        }

	        nuevoUsuario.PasswordHash = _userManager.PasswordHasher.HashPassword(nuevoUsuario, Contraseņa);

	        try
	        {
                //Creamos el nuevo usuario
		        await _userManager.CreateAsync(nuevoUsuario);

		        //Aņadimos el nuevo usuario a su rol correspondiente
		        await _userManager.AddToRoleAsync(nuevoUsuario, TipoDeUsuarioSeleccionado);

                //Guardamos los cambios
                await _dbContext.SaveChangesAsync();
	        }
	        catch (Exception ex)
	        {
		        return RedirectToPage("/Error");
	        }

	        return RedirectToPage("/Index");
        }
    }
}
