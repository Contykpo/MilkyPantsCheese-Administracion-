using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
	/// <summary>
	/// Modelo de la pagina para la edicion de <see cref="EditarLoteLecheModel"/>
	/// </summary>
	public class EditarLoteLecheModel : PageModel
    {
	    public readonly MilkyDbContext _dbContext;
	    public readonly ILogger<EditarLoteLecheModel> _logger;

		/// <summary>
		/// Id del lote de leche que estamos editando
		/// </summary>
		[BindProperty]
		public int IdLoteLecheEditado { get; set; }

		[BindProperty]
		public byte[] ImagenPlanilla { get; set; }


		public EditarLoteLecheModel(MilkyDbContext dbContext, ILogger<EditarLoteLecheModel> logger)
	    {
		    _dbContext = dbContext;
		    _logger = logger;
	    }

		public async Task OnGet([FromQuery(Name = "id")] int id)
        {
			IdLoteLecheEditado = id;

	        var lotelecheSiendoEditado = await _dbContext.LotesDeLeche.FirstOrDefaultAsync(f => f.Id == IdLoteLecheEditado);

	        if (lotelecheSiendoEditado != null)
	        {
				FechaIngreso     = lotelecheSiendoEditado.Fecha;
				PorcentajeAgua   = lotelecheSiendoEditado.PorcentajeDeAgua.ToString("F");
				Temperatura      = lotelecheSiendoEditado.Temperatura.ToString("F");
				Acidez           = lotelecheSiendoEditado.Acidez.ToString("F");
				EstaDisponible   = lotelecheSiendoEditado.EstaDisponible;
				NotasAdicionales = lotelecheSiendoEditado.NotasAdicionales;
				CisternaId       = lotelecheSiendoEditado.Cisterna.Id;
				TamboId          = lotelecheSiendoEditado.TamboDeProveniencia.Id;

				ImagenPlanilla = lotelecheSiendoEditado.ImagenPlanilla;
			}
        }

		/// <summary>
		/// Valida y aplica los cambios al lote de leche siendo editado.
		/// </summary>
		public async Task<IActionResult> OnPost()
		{
			var lotelecheSiendoEditado = await _dbContext.LotesDeLeche.ValidarIDYObtenerModelo(IdLoteLecheEditado, ModelState, nameof(IdLoteLecheEditado));

			if (lotelecheSiendoEditado is null)
				return Page();

			if (!decimal.TryParse(PorcentajeAgua, out var nuevoPorcentajeAgua))
			{
				ModelState.AddModelError(nameof(PorcentajeAgua), "El peso debe ser un valor numerico");

				return Page();
			}

			if (!decimal.TryParse(Temperatura, out var nuevaTemperatura))
			{
				ModelState.AddModelError(nameof(Temperatura), "El peso debe ser un valor numerico");

				return Page();
			}

			if (!decimal.TryParse(Acidez, out var nuevaAcidez))
			{
				ModelState.AddModelError(nameof(Acidez), "El peso debe ser un valor numerico");

				return Page();
			}

			//Si llegamos a este punto, significa que las validanciones anteriores tuvieron exito, asi que actualizamos todas las propiedades del modelo.

			var cisternas = (from c in _dbContext.Cisternas select c).ToList();
			var tambos  = (from c in _dbContext.Tambos select c).ToList();

			lotelecheSiendoEditado.Fecha               = FechaIngreso;
			lotelecheSiendoEditado.PorcentajeDeAgua    = nuevoPorcentajeAgua;
			lotelecheSiendoEditado.Temperatura         = nuevaTemperatura;
			lotelecheSiendoEditado.Acidez              = nuevaAcidez;
			lotelecheSiendoEditado.EstaDisponible      = EstaDisponible;
			lotelecheSiendoEditado.NotasAdicionales    = NotasAdicionales;
			lotelecheSiendoEditado.Cisterna            = cisternas.Single(m => m.Id == CisternaId);
			lotelecheSiendoEditado.TamboDeProveniencia = tambos.Single(m => m.Id == TamboId);

			if (ImagenPlanillita is not null)
			{
				//Guardamos los bytes de la imagen
				using (var bReader = new System.IO.BinaryReader(ImagenPlanillita.OpenReadStream()))
				{
					lotelecheSiendoEditado.ImagenPlanilla = bReader.ReadBytes((int)ImagenPlanillita.Length);
				}
			}

			//Si guardamos los datos con exito, volvemos a la pagina de administrar lotes de leches.
			if (await _dbContext.IntentarGuardarCambios(_logger, ModelState, nameof(IdLoteLecheEditado)))
				return RedirectToPage("/MilkyMan/AdministrarLotesDeLeche");

			//Si llegamos a este punto significa que ocurrio un error al intentar guardar los datos
			ModelState.AddModelError(nameof(IdLoteLecheEditado), "Ocurrio un error al intentar guardar los cambios. Si el problema persiste, contactese con soporte.");

	        return Page();
        }

		#region Propiedades para la edicion de lote de leches.

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
		[Display(Name = "Fecha de ingreso")]
		[BindProperty]
		public DateTimeOffset FechaIngreso { get; set; }

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[Display(Name = "Temperatura (°C)")]
		[BindProperty]
		public string Temperatura { get; set; } = string.Empty;

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[Display(Name = "Porcentaje de agua")]
		[BindProperty]
		public string PorcentajeAgua { get; set; } = string.Empty;

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[Display(Name = "Acidez (°D)")]
		[BindProperty]
		public string Acidez { get; set; } = string.Empty;

		[StringLength(1024)]
		[Display(Name = "Notas adicionales")]
		[BindProperty]
		public string NotasAdicionales { get; set; } = string.Empty;

		[Display(Name = "Cisterna")]
		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[BindProperty]
		public int CisternaId { get; set; }

		[Display(Name = "Tambo")]
		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[BindProperty]
		public int TamboId { get; set; }

		[Display(Name = "Esta disponible")]
		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[BindProperty]
		public bool EstaDisponible { get; set; }

		[Display(Name = "Imagen planilla")]
		[BindProperty]
		public IFormFile ImagenPlanillita { get; set; }

		#endregion
	}
}