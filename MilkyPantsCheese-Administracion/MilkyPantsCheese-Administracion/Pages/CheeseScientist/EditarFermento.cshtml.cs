using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
	/// <summary>
	/// Modelo de la pagina para la edicion de <see cref="ModeloFermento"/>
	/// </summary>
    public class EditarFermentoModel : PageModel
    {
	    public readonly MilkyDbContext _dbContext;
	    public readonly ILogger<EditarFermentoModel> _logger;

	    public EditarFermentoModel(MilkyDbContext dbContext, ILogger<EditarFermentoModel> logger)
	    {
		    _dbContext = dbContext;
		    _logger = logger;
	    }

	    [BindProperty]
	    [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
	    [DisplayName("Peso del fermento (en KG)")]
	    public string PesoFermento { get; set; }

	    [BindProperty]
	    [DisplayName("Observaciones")]
	    public string Observaciones { get; set; }

	    [BindProperty]
	    [DisplayName("Esta disponible")]
	    public bool EstaDisponible { get; set; }

		[BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [DisplayName("Fecha del fermento")]
        public DateTimeOffset FechaElaboracion { get; set; } = DateTimeOffset.Now;

		/// <summary>
		/// Id del fermento que estamos editando
		/// </summary>
        [BindProperty]
        public int IdFermentoSiendoEditado { get; set; }

        public async Task OnGet([FromQuery(Name = "id")] int id)
        {
	        IdFermentoSiendoEditado = id;

	        var fermentoSiendoEditado = await _dbContext.Fermentos.FirstOrDefaultAsync(f => f.Id == IdFermentoSiendoEditado);

	        if (fermentoSiendoEditado != null)
	        {
		        PesoFermento     = fermentoSiendoEditado.Peso.ToString("F");
		        Observaciones    = fermentoSiendoEditado.Observaciones;
		        FechaElaboracion = fermentoSiendoEditado.FechaElaboracion;
		        EstaDisponible   = fermentoSiendoEditado.EstaDisponible;
	        }
        }

		/// <summary>
		/// Valida y aplica los cambios al fermento siendo editado
		/// </summary>
		public async Task<IActionResult> OnPost()
		{
			var fermentoSiendoEditado = await _dbContext.Fermentos.ValidarIDYObtenerModelo(IdFermentoSiendoEditado, ModelState, nameof(IdFermentoSiendoEditado));

			if (fermentoSiendoEditado is null)
				return Page();

			//Intentos parsear el nuevo peso
			if (!decimal.TryParse(PesoFermento, out var nuevoPeso))
	        {
		        ModelState.AddModelError(nameof(PesoFermento), "El peso debe ser un valor numerico");

		        return Page();
	        }

			//Si llegamos a este punto, significa que las validanciones anteriores tuvieron exito, asi que actualizamos todas las propiedades del modelo
			fermentoSiendoEditado.Peso             = nuevoPeso;
	        fermentoSiendoEditado.Observaciones    = Observaciones;
	        fermentoSiendoEditado.FechaElaboracion = FechaElaboracion;
	        fermentoSiendoEditado.EstaDisponible   = EstaDisponible;

	        //Si guardamos los datos con exito, volvemos a la pagina de administrar fermentos
			if (await _dbContext.IntentarGuardarCambios(_logger, ModelState, nameof(IdFermentoSiendoEditado)))
				return RedirectToPage("/CheeseScientist/AdministrarFermentos");

			//Si llegamos a este punto significa que ocurrio un error al intentar guardar los datos
			ModelState.AddModelError(nameof(IdFermentoSiendoEditado), "Ocurrio un error al intentar guardar los cambios. Si el problema persiste, contactese con soporte");

	        return Page();
        }
    }
}