using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MilkyPantsCheese.Pages.MilkyMan
{
    /// <summary>
    /// Modelo de la pagina encargada de lidiar con la gestion de lotes de leche.
    /// </summary>
    public class AdministrarLotesDeLecheModel : PageModel
    {
        private readonly MilkyDbContext _dbContext;
        private readonly UserManager<ModeloUsuario> _userManager;

        /// <summary>
        /// Cisterna que esta siendo seleccionada por el usuario.
        /// </summary>
        public ModeloCisterna CisternaSeleccionada { get; set; }

        public AdministrarLotesDeLecheModel(MilkyDbContext dbContext, UserManager<ModeloUsuario> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPost()
        {
            //Recargamos la pagina.
            return RedirectToPage("AdministrarLotesDeLeche");
        }
    }
}
