using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina encargada de lidiar con la creacion de cisternas.
    /// </summary>
    public class CreacionCisternaModel : PageModel
    {
        private readonly MilkyDbContext _dbcontext;


        public CreacionCisternaModel(MilkyDbContext dbContext)
        {
            _dbcontext   = dbContext;
        }

        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if(!int.TryParse(CapacidadLitros, out var capacidadLitros))
                ModelState.AddModelError(nameof(CapacidadLitros), "Capacidad en litros solo puede contener caracteres numericos");

            if(ModelState.ErrorCount > 0)
                return Page();

            //Creamos la nueva cisterna.
            ModeloCisterna nuevaCisterna = new ModeloCisterna
            {
                Nombre    = Nombre,
                Capacidad = capacidadLitros
            };

            //Intentamos crear la cisterna y guardarla en la base de datos
            try
            {
                _dbcontext.Attach(nuevaCisterna).State = EntityState.Added;

                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //Si la cisterna ya se guardo en la base de datos, la borramos ya que fallo en los pasos anteriores.
                if (nuevaCisterna.Id != 0)
                    _dbcontext.Remove(nuevaCisterna);

                await _dbcontext.SaveChangesAsync();

                return new JsonResult("Algo salio mal");
            }

            //De llegar hasta aqui, significa que la creacion de la cisterna fue exitosa.
            return RedirectToPage("AdministrarLotesDeLeche");
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
