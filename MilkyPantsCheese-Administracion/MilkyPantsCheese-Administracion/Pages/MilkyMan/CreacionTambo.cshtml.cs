using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina encargada de lidiar con la creacion de tambos.
    /// </summary>
    public class CreacionTamboModel : PageModel
    {
        private readonly MilkyDbContext _dbcontext;
        private readonly UserManager<ModeloUsuario> _userManager;

        /// <summary>
        /// Tambos disponibles.
        /// </summary>
        public List<ModeloTambo> Tambos { get; set; } = new List<ModeloTambo>();

        public CreacionTamboModel(MilkyDbContext dbContext, UserManager<ModeloUsuario> userManager)
        {
            _dbcontext   = dbContext;
            _userManager = userManager;

            Tambos = (from t in _dbcontext.Tambos select t).ToList();
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

            //Creamos el nuevo tambo.
            ModeloTambo nuevoTambo = new ModeloTambo
            {
                Nombre = Nombre,
                Notas  = Notas
            };

            //Intentamos crear el tambo y guardarlo en la base de datos
            try
            {
                _dbcontext.Attach(nuevoTambo).State = EntityState.Added;

                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //Si el tambo ya se guardo en la base de datos, lo borramos ya que fallo en los pasos anteriores.
                if (nuevoTambo.Id != 0)
                    _dbcontext.Remove(nuevoTambo);

                await _dbcontext.SaveChangesAsync();

                return new JsonResult("Algo salio mal");
            }

            //Volvemos a la pagina principal.
            return RedirectToPage("CreacionTambo");
        }

        #region Propiedades para la creacion de tambos.

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [StringLength(256)]
        [BindProperty]
        public string Nombre { get; set; }

        [StringLength(1024)]
        [BindProperty]
        public string Notas { get; set; }

        #endregion
    }
}
