using System;
using System.ComponentModel.DataAnnotations;

namespace MilkyPantsCheese
{
    /// <summary>
    /// Modelo que representa un fermento.
    /// </summary>
    public class ModeloFermento : ModeloBase
    {
        /// <summary>
        /// Peso del fermentado.
        /// </summary>
        public decimal Peso { get; set; }

        /// <summary>
        /// Indica si el fermentado puede ser utilizado en un lote de queso.
        /// </summary>
        public bool EstaDisponible { get; set; }

        /// <summary>
        /// Observaciones adicionales acerca del fermentado.
        /// </summary>
        [StringLength(1024)]
        public string Observaciones { get; set; }

        /// <summary>
        /// Fecha en la que se inicio el fermentado.
        /// </summary>
        public DateTimeOffset FechaInicioFermentado { get; set; }

        /// <summary>
        /// Tipo de fermentado.
        /// </summary>
        public virtual ModeloTipoFermento TipoFermento { get; set; }
    }
}
