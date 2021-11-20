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

        /// <summary>
        /// Indica si este usuario se encuentra habilitado
        /// </summary>
        public bool EstaHabilitado { get; set; } = true;

        /// <summary>
        /// Fecha y hora en la que acaba la suspension de este usuario
        /// </summary>
        public DateTimeOffset FinSuspension { get; set; }

        public ModeloUsuario()
        {
            SecurityStamp = Guid.NewGuid().ToString();
        }
    }
}
