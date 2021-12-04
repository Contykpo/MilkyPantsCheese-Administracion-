using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina encargada de lidiar con la creacion de cisternas.
    /// </summary>
    [ValidateAntiForgeryToken]
    [Authorize(Roles = Constantes.NombreRolMilkyMan + "," + Constantes.NombreRolAdministrador)]
    public class CreacionCisternaModel : PageModel
    {
		#region Campos

		private readonly MilkyDbContext _dbContext;

		public readonly ILogger<CreacionCisternaModel> _logger; 

		#endregion

		#region Propiedades

		/// <summary>
		/// Cisternas disponibles
		/// </summary>
		public List<ModeloCisterna> Cisternas { get; set; } = new List<ModeloCisterna>(); 

		#endregion

		#region Constructor

		public CreacionCisternaModel(MilkyDbContext dbContext, ILogger<CreacionCisternaModel> logger)
		{
			_dbContext = dbContext;
			_logger = logger;

			Cisternas = (from c in _dbContext.Cisternas select c).ToList();
		} 

		#endregion

		#region Metodos

		/// <summary>
		/// Crea un nuevo <see cref="ModeloCisterna"/> con los datos ingresados por el usuario
		/// </summary>
		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
				return Page();

			if (!int.TryParse(CapacidadLitros, out var capacidadLitrosParseada))
				ModelState.AddModelError(nameof(CapacidadLitros), "Capacidad en litros solo puede contener caracteres numericos");

			if (ModelState.ErrorCount > 0)
				return Page();

			//Creamos la nueva cisterna.
			ModeloCisterna nuevaCisterna = new ModeloCisterna
			{
				Nombre = Nombre,
				Capacidad = capacidadLitrosParseada
			};

			//Intentamos crear la cisterna y guardarla en la base de datos
			if (!await _dbContext.IntentarGuardarCambios(_logger, ModelState, string.Empty, () =>
			{
				_dbContext.Add(nuevaCisterna);
			}))
			{
				_logger.LogError("Error al guardar los nuevos datos.");
			}

			//De llegar hasta aqui, significa que la creacion de la cisterna fue exitosa.
			return RedirectToPage("CreacionCisterna");
		}

		#endregion

		#region Propiedades para la creacion de cisternas.

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [StringLength(256)]
        [BindProperty]
        [DataType(DataType.Text)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Display(Name = "Capacidad en litros")]
        [StringLength(256)]
        [BindProperty]
        [DataType(DataType.Text)]
        public string CapacidadLitros { get; set; }

        #endregion
    }
}
