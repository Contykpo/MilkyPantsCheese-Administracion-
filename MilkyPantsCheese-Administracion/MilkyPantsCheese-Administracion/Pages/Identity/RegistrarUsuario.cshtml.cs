using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using MilkyPantsCheese_Administracion.Modelos;

namespace MilkyPantsCheese.Pages
{
    [Authorize(Roles = Constantes.NombreRolAdministrador)]
    public class RegistrarUsuarioModel : PageModel
    {
	    public readonly UserManager<ModeloUsuario> _userManager;

	    public RegistrarUsuarioModel()
	    {
		    
	    }

        [BindProperty]
        [PasswordPropertyText]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Compare(nameof(ConfirmacionContraseņa), ErrorMessage = "Las contraseņas deben ser iguales")]
        public string Contraseņa { get; set; }

        [BindProperty]
        [PasswordPropertyText]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        public string ConfirmacionContraseņa { get; set; }

        [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        public string NombreUsuario { get; set; }

        [BindProperty]
        [DisplayName("Tipo de usuario")]
        public string TipoDeUsuarioSeleccionado { get; set; }

        public TimeSpan DuracionSesion { get; set; } = TimeSpan.FromHours(1);

        public List<SelectListItem> TiposUsuarioDisponibles { get; set; } = UserHelpers.ObtenerTiposDeUsuarioDisponibles();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
	        
	        return Page();
        }
    }
}
