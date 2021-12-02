using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
	/// <summary>
	/// Modelo de la pagina para la edicion de <see cref="EditarCisterna"/>
	/// </summary>
	public class EditarCisterna : PageModel
    {
	    public readonly MilkyDbContext _dbContext;
	    public readonly ILogger<EditarCisterna> _logger;

		/// <summary>
		/// Id de la cisterna que estamos editando
		/// </summary>
		[BindProperty]
		public int IdCisterna { get; set; }

		public EditarCisterna(MilkyDbContext dbContext, ILogger<EditarCisterna> logger)
	    {
		    _dbContext = dbContext;
		    _logger = logger;
	    }

		public async Task OnGet([FromQuery(Name = "id")] int id)
        {
			IdCisterna = id;

	        var cisternaSiendoEditada = await _dbContext.Cisternas.FirstOrDefaultAsync(f => f.Id == IdCisterna);

	        if (cisternaSiendoEditada != null)
	        {
				Nombre          = cisternaSiendoEditada.Nombre;
				CapacidadLitros = cisternaSiendoEditada.Capacidad.ToString();
			}
        }

		/// <summary>
		/// Valida y aplica los cambios a la cisterna siendo editada.
		/// </summary>
		public async Task<IActionResult> OnPost()
		{
			var cisternaSiendoEditada = await _dbContext.Cisternas.ValidarIDYObtenerModelo(IdCisterna, ModelState, nameof(IdCisterna));

			if (cisternaSiendoEditada is null)
				return Page();

			if (!int.TryParse(CapacidadLitros, out var nuevaCapacidadLitros))
			{
				ModelState.AddModelError(nameof(CapacidadLitros), "El peso debe ser un valor numerico");

				return Page();
			}

			//Si llegamos a este punto, significa que las validanciones anteriores tuvieron exito, asi que actualizamos todas las propiedades del modelo.
			cisternaSiendoEditada.Nombre    = Nombre;
			cisternaSiendoEditada.Capacidad = nuevaCapacidadLitros;

			//Si guardamos los datos con exito, volvemos a la pagina de administrar lotes de leches.
			if (await _dbContext.IntentarGuardarCambios(_logger, ModelState, nameof(IdCisterna)))
				return RedirectToPage("/MilkyMan/CreacionCisterna");

			//Si llegamos a este punto significa que ocurrio un error al intentar guardar los datos
			ModelState.AddModelError(nameof(IdCisterna), "Ocurrio un error al intentar guardar los cambios. Si el problema persiste, contactese con soporte.");

	        return Page();
        }

		#region Propiedades para la edicion de cisternas.

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