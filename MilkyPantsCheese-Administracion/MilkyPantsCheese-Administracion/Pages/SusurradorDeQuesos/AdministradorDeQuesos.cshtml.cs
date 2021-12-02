using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina encargada de lidiar con la admnistracion de quesos.
    /// </summary>
    public class AdministradorDeQuesosModel : PageModel
    {
        private readonly MilkyDbContext _dbContext;

        public readonly ILogger<AdministradorDeQuesosModel> _logger;

        /// <summary>
        /// Lotes de queso disponibles.
        /// </summary>
        public List<ModeloLoteDeQueso> LotesDeQueso { get; set; } = new List<ModeloLoteDeQueso>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="userManager"></param>
        public AdministradorDeQuesosModel(MilkyDbContext dbContext, ILogger<AdministradorDeQuesosModel> logger)
        {
            _dbContext = dbContext;

            LotesDeQueso = (from c in dbContext.LotesDeQuesos select c).ToList();
        }

        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if(ModelState.ErrorCount > 0)
                return Page();

            var lotesLeche = (from c in _dbContext.LotesDeLeche select c).ToList();
            var tiposQuesos = (from c in _dbContext.TiposDeQuesos select c).ToList();
            var fermentos = (from c in _dbContext.Fermentos select c).ToList();

            //Creamos el nuevo lote de queso.
            ModeloLoteDeQueso nuevoLoteDeQueso = new ModeloLoteDeQueso
            {
                FechaInicioCuracion = FechaInicio, 
                Observaciones       = Observaciones,
                LoteDeLeche         = lotesLeche.Single(m => m.Id == LoteLecheId),
                TipoQueso           = tiposQuesos.Single(m => m.Id == TipoQuesoId),
                Fermento            = fermentos.Single(m => m.Id == FermentoId),
            };

            //Intentamos crear al lote de queso y guardarlo en la base de datos
            if (!await _dbContext.IntentarGuardarCambios(_logger, ModelState, string.Empty, () =>
            {
                _dbContext.Add(nuevoLoteDeQueso);
            }))
            {
                _logger.LogError("Error al guardar los nuevos datos.");
            }

            //Recargamos la pagina.
            return RedirectToPage("AdministradorDeQuesos");
        }

        #region Propiedades para la creacion / edicion de lotes de queso.

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha inicial de curacion")]
        [BindProperty]
        public DateTimeOffset FechaInicio { get; set; }

        [StringLength(1024)]
        [DataType(DataType.Text)]
        [BindProperty]
        public string Observaciones { get; set; } = string.Empty;

        [Display(Name = "Lote de leche")]
        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [BindProperty]
        public int LoteLecheId { get; set; }

        [Display(Name = "Fermento")]
        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [BindProperty]
        public int FermentoId { get; set; }

        [Display(Name = "Tipo de queso")]
        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [BindProperty]
        public int TipoQuesoId { get; set; }

        #endregion
    }
}
