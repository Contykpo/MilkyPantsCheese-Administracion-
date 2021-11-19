using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MilkyPantsCheese_Administracion.Modelos
{
    /// <summary>
    /// Clase base para el modelo de un usuario
    /// </summary>
    public class ModeloUsuario : IdentityUser<int>
    {
        /// <summary>
        /// Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }


        public List<ModeloRol> Roles { get; set; } = new List<ModeloRol>();

        public ModeloUsuario()
        {
            SecurityStamp = Guid.NewGuid().ToString();
        }
    }
}
