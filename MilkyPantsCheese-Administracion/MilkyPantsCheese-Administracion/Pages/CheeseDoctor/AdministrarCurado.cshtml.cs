using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MilkyPantsCheese.Pages
{
    public class AdministrarCuradoModel : PageModel
    {
        [BindProperty]
        [DisplayName("Id del queso")]
        public int IdQueso { get; set; }

        [BindProperty]
        [DisplayName("Tipo del queso")]
        public int IdTipoQueso { get; set; }

        [BindProperty]
        [DisplayName("Estado del queso")]
        public EEstadoQueso EstaQuesoSeleccionado { get; set; }

        [BindProperty]
        [DisplayName("Modo de comparacion")]
        public EModoComparacion ModoComparacionSeleccionado { get; set; }

        [BindProperty]
        [DisplayName("Fecha de comienzo de curado")]
        public DateTimeOffset FechaInicioCuradoSeleccionada { get; set; }

        [BindProperty]
        [DisplayName("Fecha de fin de curado")]
        public DateTimeOffset FechaFinCuradoSeleccionada { get; set; }

        /// <summary>
        /// <see cref="List{T}"/> con los quesos que satisfacen el filtro
        /// </summary>
        public List<ModeloQueso> QuesosConcordantes { get; set; }

        public async Task<PartialViewResult> OnGetActualizarFiltro()
        {


	        return Partial("_ListaQuesos");
        }
    }
}
