using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [StringLength(1024)]
        public string Observaciones { get; set; }

        /// <summary>
        /// Fecha de inicio de curacion del queso.
        /// </summary>
        public DateTime FechaInicioCuracion { get; set; }

        /// <summary>
        /// Lote de leche del que se compone.
        /// </summary>
        public ModeloLoteDeLeche LoteDeLeche { get; set; }

        /// <summary>
        /// Fermento del que se compone.
        /// </summary>
        public ModeloFermento Fermento { get; set; }

        /// <summary>
        /// Tipo del queso.
        /// </summary>
        public ModeloTipoQueso TipoQueso { get; set; }

        /// <summary>
        /// Quesos que componen al lote.
        /// </summary>
        public virtual List<ModeloQueso> Quesos { get; set; } = new List<ModeloQueso>();
    }
}
