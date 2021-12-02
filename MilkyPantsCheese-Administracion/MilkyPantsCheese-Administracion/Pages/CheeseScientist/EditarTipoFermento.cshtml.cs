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
	/// Modelo de la pagina para la edicion de <see cref="ModeloTipoFermento"/>
	/// </summary>
    public class EditarTipoFermentoModel : PageModel
    {
	    public readonly MilkyDbContext _dbContext;
	    public readonly ILogger<EditarTipoFermentoModel> _logger;
	    public EditarTipoFermentoModel(MilkyDbContext dbContext, ILogger<EditarTipoFermentoModel> logger)
	    {
		    _dbContext = dbContext;
		    _logger = logger;
	    }

	    [BindProperty]
	    [DisplayName("Nombre del fermento")]
	    [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [DataType(DataType.Text)]
	    public string Nombre { get; set; }

	    [BindProperty]
	    [DisplayName("Descripcion")]
        [DataType(DataType.Text)]
	    public string Descripcion { get; set; }

		[BindProperty]
		public int IdTipoFermentoSiendoEditado { get; set; }

        public async Task OnGet([FromQuery(Name = "id")]int id)
        {
	        IdTipoFermentoSiendoEditado = id;

	        var tipoFermento = await _dbContext.TiposDeFermento.FirstOrDefaultAsync(f => f.Id == IdTipoFermentoSiendoEditado);

	        if (tipoFermento != null)
	        {
		        Nombre      = tipoFermento.Nombre;
		        Descripcion = tipoFermento.Descripcion;
	        }
        }

		/// <summary>
		/// Guarda los cambios realizados al <see cref="ModeloTipoFermento"/> siendo editado
		/// </summary>
		public async Task<IActionResult> OnPost()
        {
	        if (!ModelState.IsValid)
		        return Page();

	        var tipoDeFermentoEncontrado =
		        await _dbContext.TiposDeFermento.ValidarIDYObtenerModelo(IdTipoFermentoSiendoEditado, ModelState, nameof(IdTipoFermentoSiendoEditado));

	        if (tipoDeFermentoEncontrado == null)
		        return Page();

	        tipoDeFermentoEncontrado.Nombre = Nombre;
	        tipoDeFermentoEncontrado.Descripcion = Descripcion;

	        if (await _dbContext.IntentarGuardarCambios(_logger, ModelState, nameof(IdTipoFermentoSiendoEditado)))
		        return RedirectToPage("/CheeseScientist/AdministrarFermentos");

	        return Page();
        }
    }
}
