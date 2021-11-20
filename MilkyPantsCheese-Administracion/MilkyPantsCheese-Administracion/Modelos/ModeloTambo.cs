using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MilkyPantsCheese
{
    /// <summary>
    /// Modelo que representa un tambo.
    /// </summary>
    public class ModeloTambo : ModeloBase
    {
        /// <summary>
        /// Nombre del tambo.
        /// </summary>
        [StringLength(256)]
        public string Nombre { get; set; }

        /// <summary>
        /// Notas acerca del tambo.
        /// </summary>
        [StringLength(1024)]
        public string Notas { get; set; }

        /// <summary>
        /// Lotes de leche provenientes de este tambo.
        /// </summary>
        public virtual List<ModeloLoteDeLeche> LotesDeLecheDeEsteTambo { get; set; } = new List<ModeloLoteDeLeche>();
    }
}
