using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
	/// <summary>
	/// Modelo de la pagina para la edicion de <see cref="ModeloTipoQueso"/>
	/// </summary>
    public class EditarTipoQuesoModel : PageModel
    {
	    public readonly MilkyDbContext _dbContext;
	    public readonly ILogger<EditarTipoQuesoModel> _logger;

	    public EditarTipoQuesoModel(MilkyDbContext dbContext, ILogger<EditarTipoQuesoModel> logger)
	    {
		    _dbContext = dbContext;
		    _logger = logger;
	    }

	    [BindProperty]
	    [DisplayName("Nombre del queso")]
	    [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
	    public string Nombre { get; set; }

		[BindProperty]
		public int IdTipoQuesoSiendoEditado { get; set; }

        public async Task OnGet([FromQuery(Name = "id")]int id)
        {
			IdTipoQuesoSiendoEditado = id;

	        var tipoQueso = await _dbContext.TiposDeQuesos.FirstOrDefaultAsync(f => f.Id == IdTipoQuesoSiendoEditado);

	        if (tipoQueso != null)
	        {
		        Nombre = tipoQueso.Nombre;
	        }
        }

		/// <summary>
		/// Guarda los cambios realizados al <see cref="ModeloTipoQueso"/> siendo editado
		/// </summary>
		public async Task<IActionResult> OnPost()
        {
	        if (!ModelState.IsValid)
		        return Page();

	        var tipoDeQuesoEncontrado = await _dbContext.TiposDeQuesos.ValidarIDYObtenerModelo(IdTipoQuesoSiendoEditado, ModelState, nameof(IdTipoQuesoSiendoEditado));

	        if (tipoDeQuesoEncontrado == null)
		        return Page();

			tipoDeQuesoEncontrado.Nombre = Nombre;

	        if (await _dbContext.IntentarGuardarCambios(_logger, ModelState, nameof(IdTipoQuesoSiendoEditado)))
		        return RedirectToPage("/SusurradorDeQuesos/CreacionTipoQueso");

	        return Page();
        }
    }
}
