using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkyPantsCheese
{
    /// <summary>
    /// Modelo que representa un tipo de queso.
    /// </summary>
    public class ModeloTipoQueso : ModeloBase
    {
        /// <summary>
        /// Nombre del tipo de queso.
        /// </summary>
        [Column(TypeName = "nvarchar(256)")]
        public string Nombre { get; set; }
    }
}
