using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina encargada de lidiar con la creacion de tambos.
    /// </summary>
    [ValidateAntiForgeryToken]
    [Authorize(Roles = Constantes.NombreRolMilkyMan)]
    public class CreacionTamboModel : PageModel
    {
		#region Campos

		private readonly MilkyDbContext _dbContext;

		public readonly ILogger<CreacionTamboModel> _logger; 

		#endregion

		#region Propiedades

		/// <summary>
		/// Tambos disponibles.
		/// </summary>
		public List<ModeloTambo> Tambos { get; set; } = new List<ModeloTambo>(); 

		#endregion

		#region Constructor

		public CreacionTamboModel(MilkyDbContext dbContext, ILogger<CreacionTamboModel> logger)
		{
			_dbContext = dbContext;
			_logger = logger;

			Tambos = (from t in _dbContext.Tambos select t).ToList();
		} 

		#endregion

		#region Metodos

		/// <summary>
		/// Crea un nuevo <see cref="ModeloTambo"/> con los datos ingresados por el usuario
		/// </summary>
		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
				return Page();

			if (ModelState.ErrorCount > 0)
				return Page();

			//Creamos el nuevo tambo.
			ModeloTambo nuevoTambo = new ModeloTambo
			{
				Nombre = Nombre,
				Notas = Notas
			};

			//Intentamos crear el tambo y guardarlo en la base de datos
			if (!await _dbContext.IntentarGuardarCambios(_logger, ModelState, string.Empty, () =>
			{
				_dbContext.Add(nuevoTambo);
			}))
			{
				_logger.LogError("Error al guardar los nuevos datos.");
			}

			//Volvemos a la pagina principal.
			return RedirectToPage("CreacionTambo");
		} 

		#endregion

		#region Propiedades para la creacion de tambos.

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [StringLength(256)]
        [BindProperty]
        [DataType(DataType.Text)]
        public string Nombre { get; set; }

        [StringLength(1024)]
        [BindProperty]
        [DataType(DataType.Text)]
        public string Notas { get; set; }

        #endregion
    }
}
