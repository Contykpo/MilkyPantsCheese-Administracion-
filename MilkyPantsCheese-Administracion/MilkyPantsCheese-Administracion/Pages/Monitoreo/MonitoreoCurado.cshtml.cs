using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MilkyPantsCheese.Pages
{
    [IgnoreAntiforgeryToken]
    [Authorize(Roles = Constantes.NombreRolAdministrador + "," + Constantes.NombreRolCheeseScientist + "," + Constantes.NombreRolSusurradorDeQuesos)]
    public class MonitoreoCuradoModel : PageModel
    {
        public readonly MilkyDbContext _dbContext;

        public MonitoreoCuradoModel(MilkyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public bool UtilizarFiltradoPorIntervalo { get; set; }

        [BindProperty]
        [DisplayName("Tiempo entre los datos")]
        public TimeSpan Intervalo { get; set; }

        public List<ModeloDatosSensorCurado> HistorialDatos { get; set; } = new List<ModeloDatosSensorCurado>();

        public PartialViewResult OnGetActualizar()
        {
            return Partial("_DatosMonitoreoCurado");
        }

        public async Task<PartialViewResult> OnPostActualizarHistorial()
        {
            if(UtilizarFiltradoPorIntervalo)
            {
                var historial = await _dbContext.DatosSensorCurado.OrderByDescending(d => d.Fecha).ToListAsync();               

                if (historial.Count > 1)
                {
                    historial.RemoveAt(0);
                }

                var historialFinal = new List<ModeloDatosSensorCurado>();

                foreach (var entradaActual in historial)
                {
                    if(historialFinal.Count == 0)
                    {
                        historialFinal.Add(entradaActual);
                    }
                    else
                    {
                        if ((historialFinal.Last().Fecha - entradaActual.Fecha) >= Intervalo)
                            historialFinal.Add(entradaActual);
                    }
                }

                HistorialDatos = historialFinal;
            }
            else
            {
                HistorialDatos = await _dbContext.DatosSensorCurado.OrderByDescending(d => d.Fecha).ToListAsync();

                if (HistorialDatos.Count > 1)
                {
                    HistorialDatos.RemoveAt(0);
                }
            }

            return Partial("_HistorialDatosSensor", this);
        }
    }
}