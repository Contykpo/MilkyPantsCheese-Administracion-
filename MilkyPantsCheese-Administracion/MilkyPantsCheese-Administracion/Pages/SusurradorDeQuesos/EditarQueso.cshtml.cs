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
	/// Modelo de la pagina para la edicion de <see cref="ModeloQueso"/>
	/// </summary>
    public class EditarQuesoModel : PageModel
    {
	    public readonly MilkyDbContext _dbContext;
	    public readonly ILogger<EditarQuesoModel> _logger;

	    public EditarQuesoModel(MilkyDbContext dbContext, ILogger<EditarQuesoModel> logger)
	    {
		    _dbContext = dbContext;
		    _logger = logger;
	    }

		/// <summary>
		/// Id del queso que estamos editando
		/// </summary>
        [BindProperty]
        public int IdQuesoEditado { get; set; }

        public async Task OnGet([FromQuery(Name = "id")] int id)
        {
			IdQuesoEditado = id;

	        var quesoSiendoEditado = await _dbContext.Quesos.FirstOrDefaultAsync(f => f.Id == IdQuesoEditado);

	        if (quesoSiendoEditado != null)
	        {
		        FechaFinalCurado = quesoSiendoEditado.FechaFinCuracion;
				EstadoQueso      = quesoSiendoEditado.EstadoQueso;
		        PesoPreCurado    = quesoSiendoEditado.PesoPreCurado.ToString("F");
		        PesoPostCurado   = quesoSiendoEditado.PesoPostCurado.ToString("F");
				LoteId           = quesoSiendoEditado.Lote.Id;
	        }
        }

		/// <summary>
		/// Valida y aplica los cambios al queso siendo editado
		/// </summary>
		public async Task<IActionResult> OnPost()
		{
			var quesoSiendoEditado = await _dbContext.Quesos.ValidarIDYObtenerModelo(IdQuesoEditado, ModelState, nameof(IdQuesoEditado));

			if (quesoSiendoEditado is null)
				return Page();

			//Intentos parsear el nuevo peso pre curado
			if (!decimal.TryParse(PesoPreCurado, out var nuevoPesoPreCurado))
	        {
		        ModelState.AddModelError(nameof(PesoPreCurado), "El peso debe ser un valor numerico");

		        return Page();
	        }

			//Intentos parsear el nuevo peso post curado
			if (!decimal.TryParse(PesoPostCurado, out var nuevoPesoPostCurado))
			{
				ModelState.AddModelError(nameof(PesoPostCurado), "El peso debe ser un valor numerico");

				return Page();
			}

			//Si llegamos a este punto, significa que las validanciones anteriores tuvieron exito, asi que actualizamos todas las propiedades del modelo

			var lotes = (from c in _dbContext.LotesDeQuesos select c).ToList();

			quesoSiendoEditado.FechaFinCuracion = FechaFinalCurado;
			quesoSiendoEditado.EstadoQueso      = EstadoQueso;
			quesoSiendoEditado.PesoPreCurado	= nuevoPesoPreCurado;
			quesoSiendoEditado.PesoPostCurado	= nuevoPesoPostCurado;
			quesoSiendoEditado.Lote             = lotes.Single(m => m.Id == LoteId);

			//Si guardamos los datos con exito, volvemos a la pagina de administrar quesos.
			if (await _dbContext.IntentarGuardarCambios(_logger, ModelState, nameof(IdQuesoEditado)))
				return RedirectToPage("/SusurradorDeQuesos/CreacionQueso");

			//Si llegamos a este punto significa que ocurrio un error al intentar guardar los datos
			ModelState.AddModelError(nameof(IdQuesoEditado), "Ocurrio un error al intentar guardar los cambios. Si el problema persiste, contactese con soporte.");

	        return Page();
        }

		#region Propiedades para la edicion de quesos.

		[DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
		[Display(Name = "Fecha de finalizacion del curado")]
		[BindProperty]
		public DateTimeOffset FechaFinalCurado { get; set; }

		[Display(Name = "Estado del queso")]
		[BindProperty]
		public EEstadoQueso EstadoQueso { get; set; }

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[Display(Name = "Peso antes del curado")]
		[StringLength(256)]
		[BindProperty]
		public string PesoPreCurado { get; set; }

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[Display(Name = "Peso despues del curado")]
		[StringLength(256)]
		[BindProperty]
		public string PesoPostCurado { get; set; }

		[Display(Name = "Lote de queso")]
		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[BindProperty]
		public int LoteId { get; set; }

		#endregion
	}
}