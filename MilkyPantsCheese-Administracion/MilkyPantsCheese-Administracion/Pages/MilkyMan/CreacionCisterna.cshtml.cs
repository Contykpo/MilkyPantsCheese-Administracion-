using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina encargada de lidiar con la creacion de cisternas.
    /// </summary>
    public class CreacionCisternaModel : PageModel
    {
        private readonly MilkyDbContext _dbContext;

        public readonly ILogger<CreacionCisternaModel> _logger;

        /// <summary>
        /// Cisternas disponibles
        /// </summary>
        public List<ModeloCisterna> Cisternas { get; set; } = new List<ModeloCisterna>();

        public CreacionCisternaModel(MilkyDbContext dbContext, ILogger<CreacionCisternaModel> logger)
        {
            _dbContext = dbContext;
            _logger    = logger;

            Cisternas = (from c in _dbContext.Cisternas select c).ToList();
        }

        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if (!ValidationHelpers.TryParseDecimal(CapacidadLitros, 5, 2, out var capacidadLitros))
                ModelState.AddModelError(nameof(CapacidadLitros), "Capacidad en litros solo puede contener caracteres numericos");

            if(ModelState.ErrorCount > 0)
                return Page();

            //Creamos la nueva cisterna.
            ModeloCisterna nuevaCisterna = new ModeloCisterna
            {
                Nombre    = Nombre
            };

            //Intentamos crear la cisterna y guardarla en la base de datos
            if (!await _dbContext.IntentarGuardarCambios(_logger, ModelState, string.Empty, () =>
            {
                _dbContext.Add(nuevaCisterna);
            }))
            {
                _logger.LogError("Error al guardar los nuevos datos.");
            }

            //De llegar hasta aqui, significa que la creacion de la cisterna fue exitosa.
            return RedirectToPage("CreacionCisterna");
        }


        #region Propiedades para la creacion de cisternas.

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [StringLength(256)]
        [BindProperty]
        public string Nombre { get; set; }

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Display(Name = "Capacidad en litros")]
        [StringLength(256)]
        [BindProperty]
        public string CapacidadLitros { get; set; }

        #endregion
    }
}
