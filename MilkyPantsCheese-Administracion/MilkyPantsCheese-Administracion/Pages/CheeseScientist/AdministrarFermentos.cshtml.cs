using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MilkyPantsCheese.Pages
{
    public class AdministrarFermentosModel : PageModel
    {
	    public readonly MilkyDbContext _dbContext;

	    public AdministrarFermentosModel(MilkyDbContext dbContext)
	    {
		    _dbContext = dbContext;
	    }

        [BindProperty]
        [DisplayName("Id del fermento")]
        public int IdFermento { get; set; }

        [BindProperty]
        [DisplayName("Nombre")]
        public string NombreTipoFermento { get; set; }

        [BindProperty]
        [DisplayName("Fecha de elaboracion")]
        public DateTimeOffset FechaElaboracion { get; set; } = DateTimeOffset.UtcNow;

        [BindProperty]
        [DisplayName("Metodo de comparacion")]
        public EModoComparacion ModoComparacionFechaSeleccionado { get; set; } = EModoComparacion.Menor;

        [BindProperty]
        [DisplayName("Vista")]
        public EVistaAdministradorFermentos VistaSeleccionada { get; set; } = EVistaAdministradorFermentos.Fermentos;

        public List<ModeloFermento> FermentosConcordantes { get; set; } = new List<ModeloFermento>();
        public List<ModeloTipoFermento> TiposDeFermentoConcordantes { get; set; } = new List<ModeloTipoFermento>();

        public async Task<IActionResult> OnPost()
        {
	        if (VistaSeleccionada == EVistaAdministradorFermentos.Fermentos)
		        return await ObtenerFermentosConcordantesConElFiltro();
            
	        return await ObtenerTiposDeFermentoConcordantesConElFiltro();
        }

        /// <summary>
        /// Ejecuta el filtro en todos los <see cref="ModeloFermento"/> y
        /// actualiza <see cref="FermentosConcordantes"/> con los que lo satisfacen
        /// </summary>
        /// <returns>Vista parcial que muestra todos los <see cref="ModeloFermento"/>
        /// que satisfacen el filtro</returns>
        private async Task<PartialViewResult> ObtenerFermentosConcordantesConElFiltro()
        {
	        IQueryable<ModeloFermento> queryFiltrarFermentos = _dbContext.Fermentos.AsQueryable();

	        if (IdFermento > 0)
		        queryFiltrarFermentos = queryFiltrarFermentos.Where(f => f.Id == IdFermento);

	        switch (ModoComparacionFechaSeleccionado)
	        {
		        case EModoComparacion.Exacto:
		        {
			        queryFiltrarFermentos = queryFiltrarFermentos.Where(f => f.FechaElaboracion == FechaElaboracion);

                    break;
		        }
		        case EModoComparacion.Mayor:
		        {
			        queryFiltrarFermentos = queryFiltrarFermentos.Where(f => f.FechaElaboracion > FechaElaboracion);

			        break;
		        }
		        case EModoComparacion.Menor:
		        {
			        queryFiltrarFermentos = queryFiltrarFermentos.Where(f => f.FechaElaboracion < FechaElaboracion);

			        break;
		        }
            }

	        FermentosConcordantes = await queryFiltrarFermentos.ToListAsync();

	        return Partial("_ListaFermentos", this);
        }

        /// <summary>
        /// Ejecuta el filtro en todos los <see cref="ModeloTipoFermento"/> y
        /// actualiza <see cref="TiposDeFermentoConcordantes"/> con los que lo satisfacen
        /// </summary>
        /// <returns>Vista parcial que muestra todos los <see cref="ModeloTipoFermento"/>
        /// que satisfacen el filtro</returns>
        private async Task<PartialViewResult> ObtenerTiposDeFermentoConcordantesConElFiltro()
        {
	        IQueryable<ModeloTipoFermento> queryFiltrarFermentos = _dbContext.TiposDeFermento.AsQueryable();

	        if (IdFermento > 0)
		        queryFiltrarFermentos = queryFiltrarFermentos.Where(f => f.Id == IdFermento);

            if(!string.IsNullOrWhiteSpace(NombreTipoFermento))
	            queryFiltrarFermentos = queryFiltrarFermentos.Where(f => f.Nombre == NombreTipoFermento);

            TiposDeFermentoConcordantes = await queryFiltrarFermentos.ToListAsync();

	        return Partial("_ListaTiposDeFermento", this);
		}
    }
}