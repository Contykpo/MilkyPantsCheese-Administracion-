using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
	/// <summary>
	/// Modelo de la pagina para la edicion de <see cref="EditarTambo"/>
	/// </summary>
	public class EditarTambo : PageModel
    {
	    public readonly MilkyDbContext _dbContext;
	    public readonly ILogger<EditarTambo> _logger;

		/// <summary>
		/// Id del tambo que estamos editando
		/// </summary>
		[BindProperty]
		public int IdTambo { get; set; }

		public EditarTambo(MilkyDbContext dbContext, ILogger<EditarTambo> logger)
	    {
		    _dbContext = dbContext;
		    _logger = logger;
	    }

		public async Task OnGet([FromQuery(Name = "id")] int id)
        {
			IdTambo = id;

	        var tamboSiendoEditado = await _dbContext.Tambos.FirstOrDefaultAsync(f => f.Id == IdTambo);

	        if (tamboSiendoEditado != null)
	        {
				Nombre = tamboSiendoEditado.Nombre;
				Notas  = tamboSiendoEditado.Notas;
			}
        }

		/// <summary>
		/// Valida y aplica los cambios al tambo siendo editado.
		/// </summary>
		public async Task<IActionResult> OnPost()
		{
			var tamboSiendoEditado = await _dbContext.Tambos.ValidarIDYObtenerModelo(IdTambo, ModelState, nameof(IdTambo));

			if (tamboSiendoEditado is null)
				return Page();

			//Si llegamos a este punto, significa que las validanciones anteriores tuvieron exito, asi que actualizamos todas las propiedades del modelo.
			tamboSiendoEditado.Nombre = Nombre;
			tamboSiendoEditado.Notas  = Notas;

			//Si guardamos los datos con exito, volvemos a la pagina de administrar lotes de leches.
			if (await _dbContext.IntentarGuardarCambios(_logger, ModelState, nameof(IdTambo)))
				return RedirectToPage("/MilkyMan/CreacionTambo");

			//Si llegamos a este punto significa que ocurrio un error al intentar guardar los datos
			ModelState.AddModelError(nameof(IdTambo), "Ocurrio un error al intentar guardar los cambios. Si el problema persiste, contactese con soporte.");

	        return Page();
        }

		#region Propiedades para la edicion de tambos.

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