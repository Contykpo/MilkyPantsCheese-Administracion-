using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina de creacion de <see cref="ModeloQueso"/>
    /// </summary>
    [Authorize(Roles = Constantes.NombreRolCheeseDoctor)]
    public class CrearQuesoModel : PageModel
    {
	    public readonly MilkyDbContext _dbContext;
	    public readonly ILogger<CrearQuesoModel> _logger;

	    public CrearQuesoModel(MilkyDbContext dbContext, ILogger<CrearQuesoModel> logger)
	    {
		    _dbContext = dbContext;
		    _logger = logger;
	    }

	    [BindProperty]
        [DisplayName("Peso pre-curado")]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        public string PesoPreCurado { get; set; }

        [BindProperty]
        [DisplayName("Peso post-curado")]
        public string PesoPostCurado { get; set; }

        [BindProperty]
        [DisplayName("Lote al que pertenece")]
        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        public int IdLoteAlQuePertenece { get; set; }

        [BindProperty]
        [DisplayName("Estado del queso")]
        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        public EEstadoQueso EstadoQueso { get; set; } = EEstadoQueso.EnCuracion;

        [BindProperty]
        [DisplayName("Fecha en la que finalizo la curacion")]
        public DateTimeOffset FechaFinCuracion { get; set; } = DateTimeOffset.MinValue;

        /// <summary>
        /// Crea un nuevo <see cref="ModeloQueso"/> con los datos ingresados por el usuario
        /// </summary>
        public async Task<IActionResult> OnPost()
        {
	        if (!ModelState.IsValid)
		        return Page();

	        //Verificamos que los valores de peso ingresados
            if(!decimal.TryParse(PesoPreCurado, out var pesoPreCuradoParseado))
                ModelState.AddModelError(nameof(PesoPreCurado), "El peso debe ser un numero decimal");

            if (!decimal.TryParse(PesoPostCurado, out var pesoPostCuradoParseado))
            {
                //Si la conversion fallo pero no se habia ingresado un numero en primer lugar entonces solamente asignamos
                //el peso a 0 y continuamos sin añadir errores
	            if (string.IsNullOrWhiteSpace(PesoPostCurado))
	            {
		            pesoPostCuradoParseado = 0;
	            }
	            else
	            {
		            ModelState.AddModelError(nameof(PesoPostCurado), "El peso debe ser un numero decimal");
                }
            }

            //Nos aseguramos de que los pesos esten dentro de los valores aceptados por la base de datos
            if(pesoPreCuradoParseado >= 1000)
                ModelState.AddModelError(nameof(PesoPreCurado), "El peso debe ser menor a 1000 y tener como maximo un decimal");

            if (pesoPostCuradoParseado >= 1000)
	            ModelState.AddModelError(nameof(PesoPostCurado), "El peso debe ser menor a 1000 y tener como maximo un decimal");

            //Intentamos obtener el lote al que pertenece este queso
            var lote = await _dbContext.LotesDeQuesos.ValidarIDYObtenerModelo(IdLoteAlQuePertenece, ModelState, nameof(IdLoteAlQuePertenece));

            //Si hay errores significa que uno o mas de los pasos anteriores fallaron asi que nos pegamos la vuelta
	        if (ModelState.ErrorCount > 0)
		        return Page();

	        var nuevoQueso = new ModeloQueso
	        {
		        EstadoQueso      = EstadoQueso,
		        PesoPreCurado    = pesoPreCuradoParseado,
		        PesoPostCurado   = pesoPostCuradoParseado,
		        Lote             = lote,
                FechaFinCuracion = FechaFinCuracion
	        };

            //Intentamos guardar los cambios y si tenemos exito volvemos a la pagina de administracion de curado
	        if (await _dbContext.IntentarGuardarCambios(_logger, ModelState, string.Empty, () => _dbContext.Add(nuevoQueso)))
		        return RedirectToPage("/CheeseDoctor/AdministrarCurado");

	        return Page();
        }
    }
}
