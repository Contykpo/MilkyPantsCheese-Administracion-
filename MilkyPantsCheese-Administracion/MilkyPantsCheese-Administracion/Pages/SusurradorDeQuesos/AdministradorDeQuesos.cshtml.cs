using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina encargada de lidiar con la admnistracion de quesos.
    /// </summary>
    [ValidateAntiForgeryToken]
    public class AdministradorDeQuesosModel : PageModel
    {
		#region Campos

		private readonly MilkyDbContext _dbContext;

		public readonly ILogger<AdministradorDeQuesosModel> _logger; 

		#endregion

		#region Propiedades

		/// <summary>
		/// Lotes de queso disponibles.
		/// </summary>
		public List<ModeloLoteDeQueso> LotesDeQueso { get; set; } = new List<ModeloLoteDeQueso>(); 

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="userManager"></param>
		public AdministradorDeQuesosModel(MilkyDbContext dbContext, ILogger<AdministradorDeQuesosModel> logger)
		{
			_dbContext = dbContext;

			LotesDeQueso = (from c in dbContext.LotesDeQuesos select c).ToList();
		} 

		#endregion

		#region Metodos

		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
				return Page();

			if (ModelState.ErrorCount > 0)
				return Page();

			var lotesLeche = (from c in _dbContext.LotesDeLeche select c).ToList();
			var tiposQuesos = (from c in _dbContext.TiposDeQuesos select c).ToList();
			var fermentos = (from c in _dbContext.Fermentos select c).ToList();

			//Creamos el nuevo lote de queso.
			ModeloLoteDeQueso nuevoLoteDeQueso = new ModeloLoteDeQueso
			{
				FechaInicioCuracion = FechaInicio,
				Observaciones = Observaciones,
				LoteDeLeche = lotesLeche.Single(m => m.Id == LoteLecheId),
				TipoQueso = tiposQuesos.Single(m => m.Id == TipoQuesoId),
				Fermento = fermentos.Single(m => m.Id == FermentoId),
			};

			//Intentamos crear al lote de queso y guardarlo en la base de datos
			if (!await _dbContext.IntentarGuardarCambios(_logger, ModelState, string.Empty, () =>
			{
				_dbContext.Add(nuevoLoteDeQueso);
			}))
			{
				_logger.LogError("Error al guardar los nuevos datos.");
			}

			//Recargamos la pagina.
			return RedirectToPage("AdministradorDeQuesos");
		} 

		#endregion

		#region Propiedades para la creacion / edicion de lotes de queso.

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Display(Name = "Fecha inicial de curacion")]
        [BindProperty]
        public DateTimeOffset FechaInicio { get; set; } = DateTimeOffset.Now;

        [StringLength(1024)]
        [DataType(DataType.Text)]
        [BindProperty]
        public string Observaciones { get; set; } = string.Empty;

        [Display(Name = "Lote de leche")]
        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [BindProperty]
        public int LoteLecheId { get; set; }

        [Display(Name = "Fermento")]
        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [BindProperty]
        public int FermentoId { get; set; }

        [Display(Name = "Tipo de queso")]
        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [BindProperty]
        public int TipoQuesoId { get; set; }

        #endregion
    }
}
