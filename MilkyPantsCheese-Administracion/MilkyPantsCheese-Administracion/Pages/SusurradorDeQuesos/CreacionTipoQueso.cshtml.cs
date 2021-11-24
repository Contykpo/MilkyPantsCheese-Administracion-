using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina encargada de lidiar con la creacion de tipos de quesos.
    /// </summary>
    public class CreacionTipoQuesoModel : PageModel
    {
        private readonly MilkyDbContext _dbContext;

        public CreacionTipoQuesoModel(MilkyDbContext dbContext)
        {
            _dbContext   = dbContext;
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
            try
            {
                _dbContext.Attach(nuevoTipoQueso).State = EntityState.Added;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //Si el tipo de queso ya se guardo en la base de datos, la borramos ya que fallo en los pasos anteriores.
                if (nuevoTipoQueso.Id != 0)
                    _dbContext.Remove(nuevoTipoQueso);

                await _dbContext.SaveChangesAsync();

                return new JsonResult("Algo salio mal");
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
