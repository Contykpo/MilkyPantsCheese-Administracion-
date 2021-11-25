using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina encargada de lidiar con la creacion de quesos.
    /// </summary>
    public class CreacionQuesoModel : PageModel
    {
        private readonly MilkyDbContext _dbContext;

        /// <summary>
        /// Quesos disponibles.
        /// </summary>
        public List<ModeloQueso> Quesos { get; set; } = new List<ModeloQueso>();

        public CreacionQuesoModel(MilkyDbContext dbContext)
        {
            _dbContext = dbContext;
        
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

            if(!int.TryParse(PesoPreCurado, out var pesoPreCurado))
                ModelState.AddModelError(nameof(PesoPreCurado), "El peso del queso precurado solo puede contener caracteres numericos");

            if(!int.TryParse(PesoPostCurado, out var pesoPostCurado))
                ModelState.AddModelError(nameof(PesoPostCurado), "El peso del queso postcurado solo puede contener caracteres numericos");

            if(ModelState.ErrorCount > 0)
                return Page();

            var lotes = (from c in _dbContext.LotesDeQuesos select c).ToList();

            //Creamos el nuevo queso.
            ModeloQueso nuevoQueso = new ModeloQueso
            {
                EstadoQueso    = EstadoQueso,
                PesoPreCurado  = pesoPreCurado,
                PesoPostCurado = pesoPostCurado,
                Lote           = lotes.Single(m => m.Id == LoteId)
            };

            //Intentamos crear el queso y guardarlo en la base de datos
            try
            {
                _dbContext.Attach(nuevoQueso).State = EntityState.Added;

                lotes.Single(m => m.Id == LoteId).Quesos.Add(nuevoQueso);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //Si el queso ya se guardo en la base de datos, la borramos ya que fallo en los pasos anteriores.
                if (nuevoQueso.Id != 0)
                    _dbContext.Remove(nuevoQueso);

                await _dbContext.SaveChangesAsync();

                return new JsonResult("Algo salio mal");
            }

            //De llegar hasta aqui, significa que la creacion del queso fue exitosa.
            return RedirectToPage("AdministradorDeQuesos");
        }


        #region Propiedades para la creacion de quesos.

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de finalizacion del curado")]
        [BindProperty]
        public DateTimeOffset FechaFinalCurado { get; set; }

        [Display(Name = "Estado del queso")]
        [BindProperty]
        public EEstadoQueso EstadoQueso { get; set; }

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Display(Name = "Peso antes del curado")]
        [StringLength(256)]
        [BindProperty]
        public string PesoPreCurado { get; set; }

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Display(Name = "Peso despues del curado")]
        [StringLength(256)]
        [BindProperty]
        public string PesoPostCurado { get; set; }

        [Display(Name = "Lote de queso")]
        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [BindProperty]
        public int LoteId { get; set; }

        #endregion
    }
}
