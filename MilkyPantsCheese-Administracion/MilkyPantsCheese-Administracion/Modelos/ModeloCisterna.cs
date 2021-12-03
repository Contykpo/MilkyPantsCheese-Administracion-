using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkyPantsCheese
{
    /// <summary>
    /// Modelo que representa a una cisterna.
    /// </summary>
    public class ModeloCisterna : ModeloBase
    {
        /// <summary>
        /// Capacidad de almacenamiento en litros de esta cisterna.
        /// </summary>
        public int Capacidad { get; set; }

        /// <summary>
        /// Nombre de la cisterna.
        /// </summary>
        [Column(TypeName = "nvarchar(256)")]
        public string Nombre { get; set; }

        /// <summary>
        /// Lotes de leche almacenados en esta cisterna.
        /// </summary>
        public virtual List<ModeloLoteDeLeche> LotesDeLeche { get; set; } = new List<ModeloLoteDeLeche>();
    }
}
