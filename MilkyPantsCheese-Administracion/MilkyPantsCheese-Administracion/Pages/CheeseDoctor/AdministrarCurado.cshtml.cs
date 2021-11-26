using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
    public class AdministrarCuradoModel : PageModel
    {
        public readonly MilkyDbContext _dbContext;
        public readonly ILogger<AdministrarCuradoModel> _logger;

        public AdministrarCuradoModel(MilkyDbContext dbContext, ILogger<AdministrarCuradoModel> logger)
        {
            _dbContext = dbContext;
            _logger    = logger;
        }

        [BindProperty]
        [DisplayName("Id del queso")]
        public int IdQueso { get; set; }

        [BindProperty]
        [DisplayName("Tipo del queso")]
        public int IdTipoQueso { get; set; }

        [BindProperty]
        [DisplayName("Peso pre-curado")]
        public string PesoPreCurado { get; set; }

        [BindProperty]
        [DisplayName("Peso post-curado")]
        public string PesoPostCurado { get; set; }

        [BindProperty]
        [DisplayName("Estado del queso")]
        public EEstadoQueso EstaQuesoSeleccionado { get; set; }

        [BindProperty]
        [DisplayName("Modo de comparacion peso")]
        public EModoComparacion ModoComparacionPesoSeleccionado { get; set; }

        [BindProperty]
        [DisplayName("Modo de comparacion fecha")]
        public EModoComparacion ModoComparacionFechaSeleccionado { get; set; }

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
            IQueryable<ModeloQueso> consulta = _dbContext.Quesos.AsQueryable();

            //Si la ID ingresada es mayor a cero, a�adimos filtrado por ID
            if (IdQueso > 0)         
                consulta = consulta.Where(q => q.Id == IdQueso);

            //Si el tipo de queso seleccionado 
            if (IdTipoQueso > 0)
                consulta = consulta.Where(q => q.Lote.TipoQueso.Id == IdTipoQueso);

            if(!ValidationHelpers.TryParseDecimal(PesoPreCurado, 4, 1, out var pesoPreCurado))
            {
                consulta = consulta.Where(q => q.PesoPreCurado == pesoPreCurado);
            }

            if (!ValidationHelpers.TryParseDecimal(PesoPreCurado, 4, 1, out var pesoPostCurado))
            {
                consulta = consulta.Where(q => q.PesoPostCurado == pesoPostCurado);
            }

            if(FechaFinCuradoSeleccionada != DateTimeOffset.MinValue)
            {
                //switch(ModoComparacionFechaSeleccionado)
                //{
                //    case EModoComparacion.Exacto:
                //        consulta
                //}
            }
            

            consulta = consulta.Where(q => q.EstadoQueso == EstaQuesoSeleccionado);

            

            return Partial("_ListaQuesos");
        }
    }
}
