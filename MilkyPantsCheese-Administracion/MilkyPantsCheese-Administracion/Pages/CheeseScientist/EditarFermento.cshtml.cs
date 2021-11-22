using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MilkyPantsCheese.Pages
{
    public class EditarFermentoModel : PageModel
    {
        public async Task OnGet([FromQuery(Name = "id")] int id)
        {
        }
    }
}
