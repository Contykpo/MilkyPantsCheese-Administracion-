using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
    public class EditarUsuarioModel : PageModel
    {
	    public readonly MilkyUserManager _userManager;
	    public readonly MilkyDbContext _dbContext;
	    public readonly ILogger<EditarUsuarioModel> _logger;

		public EditarUsuarioModel(MilkyUserManager userManager, MilkyDbContext dbContext, ILogger<EditarUsuarioModel> logger)
		{
			_userManager = userManager;
			_dbContext = dbContext;
			_logger = logger;
		}

		[BindProperty]
		[Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[DisplayName("Nombre del usuario")]
		public string NombreUsuario { get; set; }

		[BindProperty]
		[Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[DisplayName("Tipo de usuario")]
		public string RolSeleccionado { get; set; }

		[BindProperty]
		[PasswordPropertyText]
		[DisplayName("Nueva contraseña")]
		public string NuevaContraseña { get; set; }

		[BindProperty]
		[PasswordPropertyText]
		[Compare(nameof(NuevaContraseña), ErrorMessage = "Las contraseñas deben ser iguales")]
		[DisplayName("Confirmacion contraseña")]
		public string ConfirmacionNuevaContraseña { get; set; }

		[BindProperty]
		[Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[DisplayName("Duracion de la sesion (en horas)")]
		public TimeSpan DuracionSesion { get; set; } = TimeSpan.FromHours(1);

		/// <summary>
		/// Id del <see cref="ModeloUsuario"/> que esta siendo editado
		/// </summary>
		[BindProperty]
		public int IdUsuarioSiendoEditado { get; set; }

		/// <summary>
		/// Usuario que esta siendo editado
		/// </summary>
		public ModeloUsuario UsuarioSiendoEditado { get; set; }

		/// <summary>
		/// Roles disponibles
		/// </summary>
		public List<SelectListItem> RolesUsuarioDisponibles { get; set; } = UserHelpers.ObtenerTiposDeUsuarioDisponibles();

		/// <summary>
		/// Inicializa el modelo
		/// </summary>
		/// <param name="id">Id del <see cref="ModeloUsuario"/> que editaremos</param>
		public async Task OnGet(int id)
		{
			IdUsuarioSiendoEditado = id;
			UsuarioSiendoEditado = await _userManager.GetUserByIdAsync(IdUsuarioSiendoEditado);

	        if (UsuarioSiendoEditado != null)
	        {
		        RolSeleccionado = UsuarioSiendoEditado.Roles.First().Name;

		        NombreUsuario = UsuarioSiendoEditado.UserName;

		        DuracionSesion = UsuarioSiendoEditado.DuracionSesion;
	        }
        }

		/// <summary>
		/// Aplica los cambios al <see cref="ModeloUsuario"/> siendo editado
		/// </summary>
		public async Task<IActionResult> OnPost()
		{
			//Validamos el modelo
			if(!ModelState.IsValid)
				return Page();

			UsuarioSiendoEditado = await _userManager.GetUserByIdAsync(IdUsuarioSiendoEditado);

			//Nos aseguramos de que se haya podido obtener el usuario
			if (UsuarioSiendoEditado == null)
				return Page();

			UsuarioSiendoEditado.UserName       = NombreUsuario;
			UsuarioSiendoEditado.DuracionSesion = DuracionSesion;

			//Si la contraseña del usuario fue modificada...
			if (!string.IsNullOrWhiteSpace(NuevaContraseña))
			{
				foreach (var pValidator in _userManager.PasswordValidators)
				{
					if (await pValidator.ValidateAsync(_userManager, UsuarioSiendoEditado, NuevaContraseña) != IdentityResult.Success)
					{
						ModelState.AddModelError(nameof(NuevaContraseña), "La contraseña no cumple con los requisitos de seguridad");

						return Page();
					}
				}

				var hashNuevaContraseña = _userManager.PasswordHasher.HashPassword(UsuarioSiendoEditado, NuevaContraseña);

				UsuarioSiendoEditado.PasswordHash = hashNuevaContraseña;
			}

			var rolActualUsuario = UsuarioSiendoEditado.Roles.First().Name;

			//Si el rol del usuario fue modificado...
			if (rolActualUsuario != RolSeleccionado)
			{
				await _userManager.RemoveFromRoleAsync(UsuarioSiendoEditado, rolActualUsuario);
				await _userManager.AddToRoleAsync(UsuarioSiendoEditado, RolSeleccionado);
			}

			if (await _dbContext.IntentarGuardarCambios(_logger, ModelState, nameof(IdUsuarioSiendoEditado)))
				return RedirectToPage("/Administrador/AdministrarUsuarios");

			return Page();
		}
    }
}