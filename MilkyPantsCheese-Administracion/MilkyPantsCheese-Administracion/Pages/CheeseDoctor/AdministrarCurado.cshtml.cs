using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina de administracion de curado
    /// </summary>
    [Authorize(Roles = Constantes.NombreRolCheeseDoctor)]
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
        [DataType(DataType.Text)]
        public string PesoPreCurado { get; set; }

        [BindProperty]
        [DisplayName("Peso post-curado")]
        [DataType(DataType.Text)]
        public string PesoPostCurado { get; set; }

        [BindProperty]
        public bool UtilizarFiltradoPorFechaInicioCuracion { get; set; }

        [BindProperty]
        public bool UtilizarFiltradoPorFechaFinCuracion { get; set; }

        [BindProperty]
        public bool UtilizarFiltradoPorEstado { get; set; }

        [BindProperty]
        public bool UtilizarFiltradoPorTipo { get; set; }


        [BindProperty]
        [DisplayName("Estado del queso")]
        public EEstadoQueso EstadoQuesoSeleccionado { get; set; }

        [BindProperty]
        [DisplayName("Modo de comparacion peso")]
        public EModoComparacion ModoComparacionPesoSeleccionado { get; set; }

        [BindProperty]
        [DisplayName("Modo de comparacion fecha")]
        public EModoComparacion ModoComparacionFechaSeleccionado { get; set; }

        [BindProperty]
        [DisplayName("Fecha de comienzo de curado")]
        public DateTimeOffset FechaInicioCuradoSeleccionada { get; set; } = DateTimeOffset.Now;

        [BindProperty]
        [DisplayName("Fecha de fin de curado")]
        public DateTimeOffset FechaFinCuradoSeleccionada { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// <see cref="List{T}"/> con los quesos que satisfacen el filtro
        /// </summary>
        public List<ModeloQueso> QuesosConcordantes { get; set; }

        /// <summary>
        /// Ejecuta el filtro con los valores ingresados por el usuario y coloca los <see cref="ModeloQueso"/>
        /// que satisfasgan los criterios en <see cref="QuesosConcordantes"/>
        /// </summary>
        /// <returns>Partial view con los <see cref="QuesosConcordantes"/></returns>
        public async Task<PartialViewResult> OnPostActualizarFiltro()
        {
            IQueryable<ModeloQueso> consulta = _dbContext.Quesos.AsQueryable();

            //Si la ID ingresada es mayor a cero, añadimos filtrado por ID
            if (IdQueso > 0)         
                consulta = consulta.Where(q => q.Id == IdQueso);

            //Si el tipo de queso seleccionado 
            if (UtilizarFiltradoPorTipo && IdTipoQueso > 0)
                consulta = consulta.Where(q => q.Lote.TipoQueso.Id == IdTipoQueso);

            //Intentamos parsear los nuevos pesos
            if(ValidationHelpers.TryParseDecimal(PesoPreCurado, 4, 1, out var pesoPreCuradoParseado))
            {
	            consulta.AñadirWhereConTipoDeComparacion(
		            m => m.PesoPreCurado == pesoPreCuradoParseado,
		            m => m.PesoPreCurado > pesoPreCuradoParseado,
		            m => m.PesoPreCurado < pesoPreCuradoParseado,
		            ModoComparacionPesoSeleccionado);
            }

            if (ValidationHelpers.TryParseDecimal(PesoPostCurado, 4, 1, out var pesoPostCuradoParseado))
            {
	            consulta.AñadirWhereConTipoDeComparacion(
		            m => m.PesoPostCurado == pesoPostCuradoParseado,
		            m => m.PesoPostCurado > pesoPostCuradoParseado,
		            m => m.PesoPostCurado < pesoPostCuradoParseado,
		            ModoComparacionPesoSeleccionado);
            }

            //Si estamos filtrando por fecha de inicio de curacion y fecha de fin de curacion...
            if (UtilizarFiltradoPorFechaInicioCuracion &&
                UtilizarFiltradoPorFechaFinCuracion)
            {
	            consulta = consulta.Where(m =>
		            m.Lote.FechaInicioCuracion >= FechaInicioCuradoSeleccionada &&
		            m.FechaFinCuracion <= FechaFinCuradoSeleccionada);
            }
            else
            {
                //Si estamos filtrando por una sola de ellas...
	            if (UtilizarFiltradoPorFechaInicioCuracion)
	            {
		            consulta = consulta.AñadirWhereConTipoDeComparacion(
			            m => m.Lote.FechaInicioCuracion == FechaInicioCuradoSeleccionada,
			            m => m.Lote.FechaInicioCuracion > FechaInicioCuradoSeleccionada,
			            m => m.Lote.FechaInicioCuracion < FechaInicioCuradoSeleccionada,
			            ModoComparacionFechaSeleccionado);
	            }

	            if (UtilizarFiltradoPorFechaFinCuracion)
	            {
		            consulta = consulta.AñadirWhereConTipoDeComparacion(
			            m => m.FechaFinCuracion == FechaFinCuradoSeleccionada,
			            m => m.FechaFinCuracion > FechaFinCuradoSeleccionada,
			            m => m.FechaFinCuracion < FechaFinCuradoSeleccionada,
			            ModoComparacionFechaSeleccionado);
	            }
            }

            //Si estamos filtrando por el estado del queso...
            if(UtilizarFiltradoPorEstado)
	            consulta = consulta.Where(q => q.EstadoQueso == EstadoQuesoSeleccionado);

            //Obtenemos los quesos que concuerden con el filtro
            QuesosConcordantes = await consulta.ToListAsync();

            return Partial("_ListaQuesos", this);
        }
    }
}