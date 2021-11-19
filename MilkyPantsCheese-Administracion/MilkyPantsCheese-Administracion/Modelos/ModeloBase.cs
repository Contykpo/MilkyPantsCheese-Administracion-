using System.ComponentModel.DataAnnotations;

namespace MilkyPantsCheese
{
    /// <summary>
    /// Clase base de todos los modelos con una key
    /// </summary>
    public class ModeloBase
    {   
        //Id
        [Key]
        public int Id { get; set; }
    }
}
