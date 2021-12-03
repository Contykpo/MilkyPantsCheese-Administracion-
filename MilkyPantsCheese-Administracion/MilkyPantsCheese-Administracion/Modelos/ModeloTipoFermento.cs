using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "nvarchar(256)")]
        [Required]
        public string Nombre { get; set; }

        /// <summary>
        /// Descripcion del fermento
        /// </summary>
        [Column(TypeName = "nvarchar(1024)")]
        public string Descripcion { get; set; }
    }
}
