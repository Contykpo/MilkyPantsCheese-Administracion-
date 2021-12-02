using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
	/// <summary>
	/// Modelo de la pagina para la edicion de <see cref="EditarLoteQuesoModel"/>
	/// </summary>
	public class EditarLoteQuesoModel : PageModel
    {
	    public readonly MilkyDbContext _dbContext;
	    public readonly ILogger<EditarLoteQuesoModel> _logger;

	    public EditarLoteQuesoModel(MilkyDbContext dbContext, ILogger<EditarLoteQuesoModel> logger)
	    {
		    _dbContext = dbContext;
		    _logger = logger;
	    }

		/// <summary>
		/// Id del lote de queso que estamos editando
		/// </summary>
        [BindProperty]
        public int IdLoteQuesoEditado { get; set; }

        public async Task OnGet([FromQuery(Name = "id")] int id)
        {
			IdLoteQuesoEditado = id;

	        var loteQuesoSiendoEditado = await _dbContext.LotesDeQuesos.FirstOrDefaultAsync(f => f.Id == IdLoteQuesoEditado);

	        if (loteQuesoSiendoEditado != null)
	        {
		        FechaInicio   = loteQuesoSiendoEditado.FechaInicioCuracion;
				Observaciones = loteQuesoSiendoEditado.Observaciones;
		        LoteLecheId   = loteQuesoSiendoEditado.LoteDeLeche.Id;
		        FermentoId    = loteQuesoSiendoEditado.Fermento.Id;
				TipoQuesoId   = loteQuesoSiendoEditado.TipoQueso.Id;
	        }
        }

		/// <summary>
		/// Valida y aplica los cambios al lote de queso siendo editado
		/// </summary>
		public async Task<IActionResult> OnPost()
		{
			var loteQuesoSiendoEditado = await _dbContext.LotesDeQuesos.ValidarIDYObtenerModelo(IdLoteQuesoEditado, ModelState, nameof(IdLoteQuesoEditado));

			if (loteQuesoSiendoEditado is null)
				return Page();

			//Si llegamos a este punto, significa que las validanciones anteriores tuvieron exito, asi que actualizamos todas las propiedades del modelo.

			var lotesLeche = (from c in _dbContext.LotesDeLeche select c).ToList();
			var fermentos  = (from c in _dbContext.Fermentos select c).ToList();
			var tiposQueso = (from c in _dbContext.TiposDeQuesos select c).ToList();

			loteQuesoSiendoEditado.FechaInicioCuracion = FechaInicio;
			loteQuesoSiendoEditado.Observaciones       = Observaciones;
			loteQuesoSiendoEditado.LoteDeLeche         = lotesLeche.Single(m => m.Id == LoteLecheId);
			loteQuesoSiendoEditado.Fermento            = fermentos.Single(m => m.Id == FermentoId);
			loteQuesoSiendoEditado.TipoQueso           = tiposQueso.Single(m => m.Id == TipoQuesoId);

			//Si guardamos los datos con exito, volvemos a la pagina de administrar lotes de quesos.
			if (await _dbContext.IntentarGuardarCambios(_logger, ModelState, nameof(IdLoteQuesoEditado)))
				return RedirectToPage("/SusurradorDeQuesos/AdministradorDeQuesos");

			//Si llegamos a este punto significa que ocurrio un error al intentar guardar los datos
			ModelState.AddModelError(nameof(IdLoteQuesoEditado), "Ocurrio un error al intentar guardar los cambios. Si el problema persiste, contactese con soporte.");

	        return Page();
        }

		#region Propiedades para la edicion de lote de quesos.

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
		[Display(Name = "Fecha inicial de curacion")]
		[BindProperty]
		public DateTimeOffset FechaInicio { get; set; }

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