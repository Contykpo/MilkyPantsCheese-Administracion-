using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkyPantsCheese
{
    /// <summary>
    /// Modelo que representa un lote de queso.
    /// </summary>
    public class ModeloLoteDeQueso : ModeloBase
    {
        /// <summary>
        /// Observaciones adicionales acerca del lote.
        /// </summary>
        [Column(TypeName = "nvarchar(1024)")]
        public string Observaciones { get; set; }

        /// <summary>
        /// Fecha de inicio de curacion del queso.
        /// </summary>
        public DateTimeOffset FechaInicioCuracion { get; set; }

        /// <summary>
        /// Lote de leche del que se compone.
        /// </summary>
        public virtual ModeloLoteDeLeche LoteDeLeche { get; set; }

        /// <summary>
        /// Fermento del que se compone.
        /// </summary>
        public virtual ModeloFermento Fermento { get; set; }

        /// <summary>
        /// Tipo del queso.
        /// </summary>
        public virtual ModeloTipoQueso TipoQueso { get; set; }

        /// <summary>
        /// Quesos que componen al lote.
        /// </summary>
        public virtual List<ModeloQueso> Quesos { get; set; } = new List<ModeloQueso>();
    }
}
