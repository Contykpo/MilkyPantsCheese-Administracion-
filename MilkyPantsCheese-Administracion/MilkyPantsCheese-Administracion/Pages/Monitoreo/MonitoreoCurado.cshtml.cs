using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MilkyPantsCheese.Pages
{
    [Authorize(Roles = Constantes.NombreRolAdministrador + "," + Constantes.NombreRolCheeseScientist + "," + Constantes.NombreRolSusurradorDeQuesos)]
    public class MonitoreoCuradoModel : PageModel
    {
        public PartialViewResult OnGetActualizar()
        {
            return Partial("_DatosMonitoreoCurado");
        }
    }
}
