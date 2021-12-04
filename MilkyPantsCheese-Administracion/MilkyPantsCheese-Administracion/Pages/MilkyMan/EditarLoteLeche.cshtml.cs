using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
	/// <summary>
	/// Modelo de la pagina para la edicion de <see cref="EditarLoteLecheModel"/>
	/// </summary>
	[Authorize(Roles = Constantes.NombreRolMilkyMan)]
	[ValidateAntiForgeryToken]
	public class EditarLoteLecheModel : PageModel
    {
		#region Campos

		public readonly MilkyDbContext _dbContext;
		public readonly ILogger<EditarLoteLecheModel> _logger;

		/// <summary>
		/// Lote de leche siendo editado
		/// </summary>
		public ModeloLoteDeLeche loteSiendoEditado; 

		#endregion

		#region Constructores

		public EditarLoteLecheModel(MilkyDbContext dbContext, ILogger<EditarLoteLecheModel> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		} 

		#endregion

		#region Metodos

		public async Task OnGet([FromQuery(Name = "id")] int id)
		{
			IdLoteLecheEditado = id;

			var lotelecheSiendoEditado = await _dbContext.LotesDeLeche.FirstOrDefaultAsync(f => f.Id == IdLoteLecheEditado);

			if (lotelecheSiendoEditado != null)
			{
				FechaIngreso = lotelecheSiendoEditado.Fecha;
				PorcentajeAgua = lotelecheSiendoEditado.PorcentajeDeAgua.ToString("F");
				Temperatura = lotelecheSiendoEditado.Temperatura.ToString("F");
				Acidez = lotelecheSiendoEditado.Acidez.ToString("F");
				EstaDisponible = lotelecheSiendoEditado.EstaDisponible;
				NotasAdicionales = lotelecheSiendoEditado.NotasAdicionales;
				CisternaId = lotelecheSiendoEditado.Cisterna.Id;
				TamboId = lotelecheSiendoEditado.TamboDeProveniencia.Id;
			}
		}

		/// <summary>
		/// Valida y aplica los cambios al <see cref="loteSiendoEditado"/>.
		/// </summary>
		public async Task<IActionResult> OnPost()
		{
			var lotelecheSiendoEditado = await _dbContext.LotesDeLeche.ValidarIDYObtenerModelo(IdLoteLecheEditado, ModelState, nameof(IdLoteLecheEditado));

			if (lotelecheSiendoEditado is null)
				ModelState.AddModelError(string.Empty, "Algo salio mal al intentar editar este lote. Si el problema persiste, contactese con soporte.");

			if (!decimal.TryParse(PorcentajeAgua, out var nuevoPorcentajeAgua))
			{
				ModelState.AddModelError(nameof(PorcentajeAgua), "El peso debe ser un valor numerico");
			}

			if (!decimal.TryParse(Temperatura, out var nuevaTemperatura))
			{
				ModelState.AddModelError(nameof(Temperatura), "El peso debe ser un valor numerico");
			}

			if (!decimal.TryParse(Acidez, out var nuevaAcidez))
			{
				ModelState.AddModelError(nameof(Acidez), "El peso debe ser un valor numerico");
			}

			if (ModelState.ErrorCount > 0)
				return Page();

			//Si llegamos a este punto, significa que las validanciones anteriores tuvieron exito, asi que actualizamos todas las propiedades del modelo.

			var cisternas = (from c in _dbContext.Cisternas select c).ToList();
			var tambos = (from c in _dbContext.Tambos select c).ToList();

			lotelecheSiendoEditado.Fecha = FechaIngreso;
			lotelecheSiendoEditado.PorcentajeDeAgua = nuevoPorcentajeAgua;
			lotelecheSiendoEditado.Temperatura = nuevaTemperatura;
			lotelecheSiendoEditado.Acidez = nuevaAcidez;
			lotelecheSiendoEditado.EstaDisponible = EstaDisponible;
			lotelecheSiendoEditado.NotasAdicionales = NotasAdicionales;
			lotelecheSiendoEditado.Cisterna = cisternas.Single(m => m.Id == CisternaId);
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

		#endregion

		#region Propiedades para la edicion de lote de leches.

		/// <summary>
		/// Id del lote de leche que estamos editando
		/// </summary>
		[BindProperty]
		public int IdLoteLecheEditado { get; set; }

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[Display(Name = "Fecha de ingreso")]
		[BindProperty]
		public DateTimeOffset FechaIngreso { get; set; }

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[Display(Name = "Temperatura (°C)")]
		[BindProperty]
        [DataType(DataType.Text)]
		public string Temperatura { get; set; } = string.Empty;

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[Display(Name = "Porcentaje de agua")]
		[BindProperty]
        [DataType(DataType.Text)]
		public string PorcentajeAgua { get; set; } = string.Empty;

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[Display(Name = "Acidez (°D)")]
		[BindProperty]
        [DataType(DataType.Text)]
		public string Acidez { get; set; } = string.Empty;

		[StringLength(1024)]
		[Display(Name = "Notas adicionales")]
		[BindProperty]
        [DataType(DataType.Text)]
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