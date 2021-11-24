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
using Microsoft.Identity.Client;

namespace MilkyPantsCheese.Pages
{
    public class EditarQuesoModel : PageModel
    {
	    public readonly MilkyDbContext _dbContext;
	    public readonly ILogger<EditarQuesoModel> _logger;

	    public EditarQuesoModel(MilkyDbContext dbContext, ILogger<EditarQuesoModel> logger)
	    {
		    _dbContext = dbContext;
		    _logger = logger;
	    }

	    [BindProperty]
	    [DisplayName("Peso pre-curado")]
	    public string PesoPreCurado { get; set; }

	    [BindProperty]
	    [DisplayName("Peso post-curado")]
	    public string PesoPostCurado { get; set; }

	    [BindProperty]
	    [DisplayName("Lote al que pertenece")]
	    public int IdLoteAlQuePertenece { get; set; }

	    [BindProperty]
	    [DisplayName("Estado del queso")]
	    public EEstadoQueso EstadoQueso { get; set; } = EEstadoQueso.EnCuracion;

	    [BindProperty]
	    [DisplayName("Fecha en la que finalizo la curacion")]
	    public DateTimeOffset FechaFinCuracion { get; set; } = DateTimeOffset.MinValue;

        [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ocurrio un error al obtener la id del queso siendo editado.")]
        public int IdQuesoSiendoEditado { get; set; }

        public async void OnGet([FromQuery(Name = "id")] int idQueso)
        {
	        IdQuesoSiendoEditado = idQueso;

	        var queso = await _dbContext.Quesos.FirstOrDefaultAsync(q => q.Id == IdQuesoSiendoEditado);

	        if (queso != null)
	        {
		        PesoPreCurado = queso.PesoPreCurado.ToString("F");
		        PesoPostCurado = queso.PesoPostCurado.ToString("F");

		        EstadoQueso = queso.EstadoQueso;

		        FechaFinCuracion = queso.FechaFinCuracion;

		        IdLoteAlQuePertenece = queso.Lote.Id;
	        }
        }
		
		/// <summary>
		/// Actualiza los campos del <see cref="ModeloQueso"/> sinedo editado con los datos ingresados por el usuario
		/// </summary>
		public async Task<IActionResult> OnUpdate()
        {
	        if (!ModelState.IsValid)
		        return Page();

	        var quesoSiendoEditado = await _dbContext.Quesos.ValidarIDYObtenerModelo(IdQuesoSiendoEditado, ModelState, nameof(IdQuesoSiendoEditado));
	        var loteAlQuePertenece = await _dbContext.LotesDeQuesos.ValidarIDYObtenerModelo(IdLoteAlQuePertenece, ModelState, nameof(IdLoteAlQuePertenece));

	        if(!PesoPreCurado.TryParseDecimal( 4, 1, out var nuevoPesoPreCurado))
				ModelState.AddModelError(nameof(PesoPreCurado), "El valor ingresado debe ser un numero decimal");

			if (!PesoPreCurado.TryParseDecimal(4, 1, out var nuevoPesoPostCurado))
				ModelState.AddModelError(nameof(PesoPostCurado), "El valor ingresado debe ser un numero decimal");

			if (ModelState.ErrorCount > 0)
				return Page();

			quesoSiendoEditado.PesoPreCurado  = nuevoPesoPreCurado;
			quesoSiendoEditado.PesoPostCurado = nuevoPesoPostCurado;
			quesoSiendoEditado.FechaFinCuracion = FechaFinCuracion;
			quesoSiendoEditado.Lote = loteAlQuePertenece;

			if (await _dbContext.IntentarGuardarCambios(_logger, ModelState, string.Empty))
				return RedirectToPage("/CheeseDoctor/AdministrarCurado");

			return Page();
        }
    }
}