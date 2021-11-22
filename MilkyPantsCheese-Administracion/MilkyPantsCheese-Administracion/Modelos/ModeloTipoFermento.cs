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
        [Required]
        public string Nombre { get; set; }

        /// <summary>
        /// Descripcion del fermento
        /// </summary>
        [StringLength(1024)]
        public string Descripcion { get; set; }
    }
}
