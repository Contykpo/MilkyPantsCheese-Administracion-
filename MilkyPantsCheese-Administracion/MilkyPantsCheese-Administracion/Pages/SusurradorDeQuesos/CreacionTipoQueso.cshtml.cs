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
    /// Modelo de la pagina encargada de lidiar con la creacion de tipos de quesos.
    /// </summary>
    public class CreacionTipoQuesoModel : PageModel
    {
        private readonly MilkyDbContext _dbContext;

        public readonly ILogger<CreacionTipoQuesoModel> _logger;

        /// <summary>
        /// Tipos de quesos disponibles.
        /// </summary>
        public List<ModeloTipoQueso> TiposQuesos { get; set; } = new List<ModeloTipoQueso>();

        public CreacionTipoQuesoModel(MilkyDbContext dbContext, ILogger<CreacionTipoQuesoModel> logger)
        {
            _dbContext = dbContext;
            _logger    = logger;

            TiposQuesos = (from c in dbContext.TiposDeQuesos select c).ToList();
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

            //Creamos el nuevo tipo de queso.
            ModeloTipoQueso nuevoTipoQueso = new ModeloTipoQueso
            {
                Nombre = NombreTipoQueso
            };

            //Intentamos crear el tipo de queso y guardarlo en la base de datos
            if (!await _dbContext.IntentarGuardarCambios(_logger, ModelState, string.Empty, () =>
            {
                _dbContext.Add(nuevoTipoQueso);
            }))
            {
                _logger.LogError("Error al guardar los nuevos datos.");
            }

            //De llegar hasta aqui, significa que la creacion del tipo de queso fue exitosa.
            return RedirectToPage("CreacionTipoQueso");
        }


        #region Propiedades para la creacion de tipos de quesos.

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Display(Name = "Nombre del queso")]
        [StringLength(256)]
        [BindProperty]
        public string NombreTipoQueso { get; set; }

        #endregion
    }
}
