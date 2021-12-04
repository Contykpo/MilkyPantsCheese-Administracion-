using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina encargada de lidiar con la gestion de lotes de leche.
    /// </summary>
    [ValidateAntiForgeryToken]
	[Authorize(Roles = Constantes.NombreRolMilkyMan)]
    public class AdministrarLotesDeLecheModel : PageModel
    {
		#region Campos

		private readonly MilkyDbContext _dbContext;

		private readonly IConfiguration _config;

		public readonly ILogger<AdministrarLotesDeLecheModel> _logger; 

		#endregion

		#region Propiedades

		/// <summary>
		/// Cisterna seleccionada para mostrar sus lotes.
		/// </summary>
		[Display(Name = "Cisterna")]
		[BindProperty]
		public int CisternaLotesId { get; set; }

		/// <summary>
		/// Lotes de leche disponibles.
		/// </summary>
		public List<ModeloLoteDeLeche> LotesDeLeche { get; set; } = new List<ModeloLoteDeLeche>(); 

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="userManager"></param>
		public AdministrarLotesDeLecheModel(MilkyDbContext dbContext, IConfiguration config, ILogger<AdministrarLotesDeLecheModel> logger)
		{
			_dbContext = dbContext;
			_config = config;
			_logger = logger;

			LotesDeLeche = (from c in dbContext.LotesDeLeche select c).ToList();
		} 

		#endregion

		#region Metodos

		/// <summary>
		/// Crea un nuevo <see cref="ModeloLoteDeLeche"/> con los datos ingresados por el usuario
		/// </summary>
		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
				return Page();

			if (!ValidationHelpers.TryParseDecimal(PorcentajeAgua, 5, 2, out var porcentajeAgua))
				ModelState.AddModelError(nameof(PorcentajeAgua), "Porcentaje de agua solo puede contener caracteres numericos");

			if (!ValidationHelpers.TryParseDecimal(Temperatura, 5, 2, out var temperatura))
				ModelState.AddModelError(nameof(Temperatura), "Temperatura solo puede contener caracteres numericos");

			if (!ValidationHelpers.TryParseDecimal(Acidez, 5, 2, out var acidez))
				ModelState.AddModelError(nameof(Acidez), "Acidez solo puede contener caracteres numericos");

			if (ImagenPlanillita is not null)
			{
				//Obtenemos el tamaño maximo permitido para un archivo desde la configuracion
				var tamañoMaximoImagen = int.Parse(_config["TamanioMaximoArchivo"]);

				//Nos aseguramos de que el tamaño del archivo subido no sea mayor al maximo
				if (ImagenPlanillita.Length > tamañoMaximoImagen)
					ModelState.AddModelError($"{nameof(ImagenPlanillita)}", "Imagen demasiado grande");

				//Nos aseguramos de que el formato del archivo subido sea valido
				if (!ImagenPlanillita.ImagenEsValida())
					ModelState.AddModelError($"{nameof(ImagenPlanillita)}", "Imagen no valida");
			}

			if (ModelState.ErrorCount > 0)
				return Page();

			var cisternas = (from c in _dbContext.Cisternas select c).ToList();
			var tambos = (from c in _dbContext.Tambos select c).ToList();

			//Creamos el nuevo lote de leche.
			ModeloLoteDeLeche nuevoLoteDeLeche = new ModeloLoteDeLeche
			{
				Fecha = FechaIngreso,
				PorcentajeDeAgua = porcentajeAgua,
				Temperatura = temperatura,
				Acidez = acidez,
				EstaDisponible = EstaDisponible,
				NotasAdicionales = NotasAdicionales,
				Cisterna = cisternas.Single(m => m.Id == CisternaId),
				TamboDeProveniencia = tambos.Single(m => m.Id == TamboId)
			};

			if (ImagenPlanillita is not null)
			{
				//Guardamos los bytes de la imagen
				using (var bReader = new BinaryReader(ImagenPlanillita.OpenReadStream()))
				{
					nuevoLoteDeLeche.ImagenPlanilla = bReader.ReadBytes((int)ImagenPlanillita.Length);
				}
			}

			//Intentamos crear al lote de leche y guardarlo en la base de datos
			if (!await _dbContext.IntentarGuardarCambios(_logger, ModelState, string.Empty, () =>
			{
				_dbContext.Add(nuevoLoteDeLeche);

				cisternas.Single(m => m.Id == CisternaId).LotesDeLeche.Add(nuevoLoteDeLeche);

				tambos.Single(m => m.Id == TamboId).LotesDeLecheDeEsteTambo.Add(nuevoLoteDeLeche);
			}))
			{
				_logger.LogError("Error al guardar los nuevos datos.");
			}

			//Recargamos la pagina.
			return RedirectToPage("AdministrarLotesDeLeche");
		}

		/// <summary>
		/// Devuelve una <see cref="PartialViewResult"/> con la lista de los <see cref="ModeloLoteDeLeche"/> que satisfacen el filtro ingresado
		/// </summary>
		/// <returns><see cref="PartialViewResult"/> con la lista de los <see cref="ModeloLoteDeLeche"/> que satisfacen el filtro ingresado</returns>
		public PartialViewResult OnPostFiltradoLotes()
		{
			return Partial("_Lotes", this);
		} 

		#endregion

		#region Propiedades para la creacion de lotes de leche.

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
		[Display(Name = "Fecha de ingreso")]
		[DataType(DataType.Text)]
		[BindProperty]
		public DateTimeOffset FechaIngreso { get; set; }

		[Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Display(Name = "Temperatura (°C)")]
        [DataType(DataType.Text)]
        [BindProperty]
        public string Temperatura { get; set; } = string.Empty;

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Display(Name = "Porcentaje de agua")]
        [DataType(DataType.Text)]
        [BindProperty]
        public string PorcentajeAgua { get; set; } = string.Empty;

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Display(Name = "Acidez (°D)")]
        [DataType(DataType.Text)]
        [BindProperty]
        public string Acidez { get; set; } = string.Empty;

        [StringLength(1024)]
        [Display(Name = "Notas adicionales")]
        [DataType(DataType.Text)]
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
        [BindProperty]
        public bool EstaDisponible { get; set; } = true;

        [Display(Name = "Imagen planilla")]
        [BindProperty]
        public IFormFile ImagenPlanillita { get; set; }

        #endregion
    }
}