using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina para crear <see cref="ModeloFermento"/>
    /// </summary>
    public class CrearFermentoModel : PageModel
    {
	    public readonly MilkyDbContext _dbContext;
	    public readonly ILogger<CrearFermentoModel> _logger;

	    public CrearFermentoModel(MilkyDbContext dbContext, ILogger<CrearFermentoModel> logger)
	    {
		    _dbContext = dbContext;
		    _logger = logger;
	    }

        [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [DisplayName("Peso del fermento")]
        public string PesoFermento { get; set; }

        [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [DisplayName("Obtener fecha automaticamente")]
        public bool ObtenerFechaAutomaticamente { get; set; } = true;

        [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [DisplayName("Fecha del fermento")]
        public DateTimeOffset FechaFermento { get; set; } = DateTimeOffset.Now;

        [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [DisplayName("Tipo de fermento")]
        public string IdTipoDeFermentoSeleccionado { get; set; }

        [BindProperty]
        [DisplayName("Observaciones")]
        public string Observaciones { get; set; }

        public List<SelectListItem> TiposDeFermentoDisponibles { get; set; }

        public async Task OnGet()
        {
	        TiposDeFermentoDisponibles = await _dbContext.ObtenerSelectListDeTiposDeFermentos();
        }

        public async Task<IActionResult> OnPost()
        {
	        if (!ModelState.IsValid)
		        return Page();

            //Intentamos parsear el peso
	        if (!decimal.TryParse(PesoFermento, out var pesoParseado))
		        ModelState.AddModelError(nameof(PesoFermento), "El peso debe ser un numero");

            //Intentamos parsear la id del tipo de fermento seleccionado
	        if (!int.TryParse(IdTipoDeFermentoSeleccionado, out var idTipoDeFermentoParseada))
                ModelState.AddModelError(nameof(IdTipoDeFermentoSeleccionado), "Hay un error con la Id del tipo de fermento");

            //Si hay errores significa que alguno de los tryparse anteriores fallo asi que nos pegamos la vuelta
	        if (ModelState.ErrorCount > 0)
		        return Page();

            //Intentamos obtener el tipo de fermento con la id especificada
	        var tipoDeFermento = await _dbContext.TiposDeFermento.FirstOrDefaultAsync(t => t.Id == idTipoDeFermentoParseada);

            //Si fallo añadimos un error y nos peamos la vuelta
	        if (tipoDeFermento == null)
	        {
                ModelState.AddModelError(nameof(IdTipoDeFermentoSeleccionado), "No se pudo encontrar el tipo de fermento especificado");

                return Page();
	        }
	        
	        var nuevoFermento = new ModeloFermento
		    {
			    EstaDisponible = true,
			    FechaElaboracion = FechaFermento,
			    Observaciones = Observaciones,
                Peso = pesoParseado,
                TipoFermento = tipoDeFermento
		    };

	        try
	        {
		        await _dbContext.AddAsync(nuevoFermento);

		        await _dbContext.SaveChangesAsync();

		        return RedirectToPage("/CheeseScientist/AdministrarFermentos");
	        }
	        catch (Exception ex)
	        {
                _logger.LogError(ex, "Ocurrio un error al intentar crear un nuevo fermento");
	        }
	        
	        return Page();
        }
    }
}
