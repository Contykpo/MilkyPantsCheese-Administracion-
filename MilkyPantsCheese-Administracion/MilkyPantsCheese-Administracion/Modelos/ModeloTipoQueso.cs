using System.ComponentModel.DataAnnotations;

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
        [StringLength(256)]
        public string Nombre { get; set; }
    }
}
