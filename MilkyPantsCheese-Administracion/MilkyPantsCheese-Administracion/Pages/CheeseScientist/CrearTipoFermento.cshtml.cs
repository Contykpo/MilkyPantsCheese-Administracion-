using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
	/// <summary>
	/// Modelo de la pagina para crear <see cref="ModeloTipoFermento"/>
	/// </summary>
    public class CrearTipoFermentoModel : PageModel
    {
	    public readonly MilkyDbContext _dbContext;
	    public readonly ILogger<CrearTipoFermentoModel> _logger;

	    public CrearTipoFermentoModel(MilkyDbContext dbContext, ILogger<CrearTipoFermentoModel> logger)
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

        public async Task<IActionResult> OnPost()
        {
	        if (!ModelState.IsValid)
		        return Page();

	        var modeloTipoFermento = new ModeloTipoFermento
	        {
		        Nombre = Nombre,
		        Descripcion = Descripcion
	        };

	        try
	        {
		        await _dbContext.AddAsync(modeloTipoFermento);

		        await _dbContext.SaveChangesAsync();

		        return RedirectToPage("/CheeseScientist/AdministrarFermentos");
	        }
	        catch (Exception ex)
	        {
				_logger.LogError(ex, "Error al crear un nuevo tipo de fermento");
	        }

	        return Page();
        }
    }
}
