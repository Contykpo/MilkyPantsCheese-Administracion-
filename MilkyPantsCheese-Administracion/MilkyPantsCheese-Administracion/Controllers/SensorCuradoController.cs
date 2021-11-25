using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilkyPantsCheese
{
    [Route("~/api/[controller]")]
    [ApiController]
    public class SensorCuradoController : ControllerBase
    {
        public readonly MilkyDbContext _dbContext;
        public readonly ILogger<SensorCuradoController> _logger;

        public SensorCuradoController(MilkyDbContext dbContext, ILogger<SensorCuradoController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        // POST api/<SensorCuradoController>
        [HttpPost("~/api/[controller]")]
        public async void Post([FromQuery(Name = "t")] string temperatura, [FromQuery(Name = "h")] string humedad, [FromQuery(Name = "d")] string dioxidoDeCarbono)
        {
            if(!ValidationHelpers.TryParseDecimal(temperatura, 4, 1, out var temperaturaParseada))
            {
                _logger.LogError($"{nameof(temperatura)} no valida");
            }

            if(!ValidationHelpers.TryParseDecimal(humedad, 4, 1, out var humedadParseada))
            {
                _logger.LogError($"{nameof(humedad)} no valida");
            }

            if(ValidationHelpers.TryParseDecimal(dioxidoDeCarbono, 10, 5, out var dioxidoDeCarbonoParseado))
            {
                _logger.LogError($"{nameof(dioxidoDeCarbono)} no valido");
            }

            var nuevosDatosSensor = new ModeloDatosSensorCurado
            {
                Temperatura      = temperaturaParseada,
                Humedad          = humedadParseada,
                DioxidoDeCarbono = dioxidoDeCarbonoParseado,
                Fecha            = DateTimeOffset.UtcNow
            };

            if(!await _dbContext.IntentarGuardarCambios(_logger, ModelState, string.Empty, () => _dbContext.Add(nuevosDatosSensor)))
            {
                _logger.LogError("Error al guardar los nuevos datos del sensor");
            }
        }
    }
}
