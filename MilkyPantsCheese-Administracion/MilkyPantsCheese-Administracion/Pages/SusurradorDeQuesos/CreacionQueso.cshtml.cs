using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina encargada de lidiar con la creacion de quesos.
    /// </summary>
    public class CreacionQuesoModel : PageModel
    {
        private readonly MilkyDbContext _dbContext;

        public readonly ILogger<CreacionQuesoModel> _logger;

        /// <summary>
        /// Quesos disponibles.
        /// </summary>
        public List<ModeloQueso> Quesos { get; set; } = new List<ModeloQueso>();

        public CreacionQuesoModel(MilkyDbContext dbContext, ILogger<CreacionQuesoModel> logger)
        {
            _dbContext = dbContext;
            _logger    = logger;
        
            Quesos = (from c in dbContext.Quesos select c).ToList();
        }

        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if (!ValidationHelpers.TryParseDecimal(PesoPreCurado, 5, 2, out var pesoPreCurado))
                ModelState.AddModelError(nameof(PesoPreCurado), "El peso del queso precurado solo puede contener caracteres numericos");

            if (!decimal.TryParse(PesoPostCurado, out var pesoPostCurado))
            {
                //Si la conversion fallo, pero no se habia ingresado un numero en primer lugar entonces solamente asignamos
                //el peso a 0 y continuamos sin añadir errores
                if (string.IsNullOrWhiteSpace(PesoPostCurado))
                    pesoPostCurado = 0;
                else
                    ModelState.AddModelError(nameof(PesoPostCurado), "El peso debe ser un numero decimal");
            }

            if (ModelState.ErrorCount > 0)
                return Page();

            var lotes = (from c in _dbContext.LotesDeQuesos select c).ToList();

            //Creamos el nuevo queso.
            ModeloQueso nuevoQueso = new ModeloQueso
            {
                FechaFinCuracion = FechaFinalCurado,
                EstadoQueso    = EstadoQueso,
                PesoPreCurado  = pesoPreCurado,
                PesoPostCurado = pesoPostCurado,
                Lote           = lotes.Single(m => m.Id == LoteId)
            };

            //Intentamos crear el queso y guardarlo en la base de datos
            if (!await _dbContext.IntentarGuardarCambios(_logger, ModelState, string.Empty, () =>
            {
                _dbContext.Add(nuevoQueso);

                lotes.Single(m => m.Id == LoteId).Quesos.Add(nuevoQueso);
            }))
            {
                _logger.LogError("Error al guardar los nuevos datos.");
            }
            
            //De llegar hasta aqui, significa que la creacion del queso fue exitosa.
            return RedirectToPage("CreacionQueso");
        }


        #region Propiedades para la creacion de quesos.

        [BindProperty]
        [DisplayName("Fecha en la que finalizo la curacion")]
        public DateTimeOffset FechaFinalCurado { get; set; } = DateTimeOffset.MinValue;

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Display(Name = "Estado del queso")]
        [BindProperty]
        public EEstadoQueso EstadoQueso { get; set; } = EEstadoQueso.EnCuracion;

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Display(Name = "Peso antes del curado")]
        [StringLength(256)]
        [DataType(DataType.Text)]
        [BindProperty]
        public string PesoPreCurado { get; set; }

        [Display(Name = "Peso despues del curado")]
        [StringLength(256)]
        [DataType(DataType.Text)]
        [BindProperty]
        public string PesoPostCurado { get; set; }

        [Display(Name = "Lote de queso")]
        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [BindProperty]
        public int LoteId { get; set; }

        #endregion
    }
}
