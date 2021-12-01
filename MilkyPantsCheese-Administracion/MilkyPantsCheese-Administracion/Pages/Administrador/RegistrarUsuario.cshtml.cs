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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina para registrar usuarios
    /// </summary>
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

        public List<SelectListItem> TiposUsuarioDisponibles { get; set; } = DecenciaHelpers.ObtenerTiposDeUsuarioDisponibles();

        /// <summary>
        /// Obtiene un <see cref="ContentResult"/> que indica si existe un usuario con el <paramref name="nombre"/> especificado
        /// </summary>
        /// <param name="nombre">Nombre que buscar</param>
        /// <returns>Cadena de texto que indica si existe o no un usuario con el <param name="nombre"> especificado</param></returns>
        public async Task<ContentResult> OnGetExisteUnUsuarioConEsteNombre([FromQuery(Name = "nombre")]string nombre)
        {
	        return new ContentResult
	        {
		        ContentType = "text/plain",
                Content = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == nombre) == null 
	                ? Constantes.FalseString 
	                : Constantes.TrueString
	        };
        }

        /// <summary>
        /// Crea un nuevo <see cref="ModeloUsuario"/> con los datos ingresados
        /// </summary>
        public async Task<IActionResult> OnPost()
        {
	        if (!ModelState.IsValid)
		        return Page();

            //Nos aseguramos de que no exista un usuario con el mismo nombre
	        if (await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == NombreUsuario) != null)
	        {
                ModelState.AddModelError(nameof(NombreUsuario), "Ya existe un usuario con ese nombre");

                return Page();
	        }

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

	        //Creamos el nuevo usuario
	        await _userManager.CreateAsync(nuevoUsuario);

	        //Añadimos el nuevo usuario a su rol correspondiente
	        await _userManager.AddToRoleAsync(nuevoUsuario, TipoDeUsuarioSeleccionado);

	        //Guardamos los cambios
	        await _dbContext.SaveChangesAsync();

            if (await _dbContext.IntentarGuardarCambios(_logger, ModelState, string.Empty))
	        {
                //Si tenemos exito al crear el usuario redireccionamos al usuario a la pagina de administracion de usuarios
		        return RedirectToPage("/Administrador/AdministrarUsuarios");
            }

	        return Page();
        }
    }
}
