using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MilkyPantsCheese
{
    /// <summary>
    /// Modelo para un rol en Milky Pants Cheese
    /// </summary>
    public class ModeloRol : IdentityRole<int>
    {
	    /// <summary>
	    /// Usuarios que tienen este rol
	    /// </summary>
	    public virtual List<ModeloUsuario> Usuarios { get; set; } = new List<ModeloUsuario>();

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ModeloRol(){}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nombre">Nombre del rol</param>
        public ModeloRol(string nombre)
        {
            Name = nombre;
        }
    }
}
