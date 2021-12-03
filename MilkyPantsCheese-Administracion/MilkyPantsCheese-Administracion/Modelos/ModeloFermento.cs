using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "nvarchar(1024)")]
        public string Observaciones { get; set; }

        /// <summary>
        /// Fecha en la que se elaboro el fermento.
        /// </summary>
        public DateTimeOffset FechaElaboracion { get; set; }

        /// <summary>
        /// Tipo de fermentado.
        /// </summary>
        public virtual ModeloTipoFermento TipoFermento { get; set; }

        /// <summary>
        /// Lote de leche en el que se utilizo este fermento
        /// </summary>
        public virtual ModeloLoteDeLeche LoteEnElQueSeLoUtilizo { get; set; }
    }
}
