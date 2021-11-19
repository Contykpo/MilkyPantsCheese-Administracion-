using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MilkyPantsCheese_Administracion.Modelos
{
    /// <summary>
    /// Modelo para un rol en Milky Pants Cheese
    /// </summary>
    public class ModeloRol : IdentityRole<int>
    {
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
