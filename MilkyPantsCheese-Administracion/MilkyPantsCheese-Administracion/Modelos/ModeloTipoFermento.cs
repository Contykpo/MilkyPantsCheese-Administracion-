using System.ComponentModel.DataAnnotations;

namespace MilkyPantsCheese
{
    /// <summary>
    /// Modelo que representa un tipo de fermento.
    /// </summary>
    public class ModeloTipoFermento : ModeloBase
    {
        /// <summary>
        /// Nombre del tipo de fermento.
        /// </summary>
        [StringLength(256)]
        public string Nombre { get; set; }
    }
}
