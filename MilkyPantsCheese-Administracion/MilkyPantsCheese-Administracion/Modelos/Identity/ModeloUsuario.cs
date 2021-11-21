using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MilkyPantsCheese
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
        /// <para>
        ///     Indica si el usuario tiene la sesion abierta.
        /// </para>
        /// <para>
        ///     ¡No utilizar para verificar si un usuario esta logueado! En su lugar utilizar el metodo <see cref="MilkySignInManager.IsSignedIn"/>
        /// </para>
        /// </summary>
        public bool TieneSesionAbierta { get; set; }

        /// <summary>
        /// Fecha y hora en la que acaba la suspension de este usuario
        /// </summary>
        public DateTimeOffset FinSuspension { get; set; }

        /// <summary>
        /// Duracion de la sesion del usuario
        /// </summary>
        public TimeSpan DuracionSesion { get; set; }

        /// <summary>
        /// Fecha y hora del ultimo inicio de sesion
        /// </summary>
        public virtual List<ModeloDateTimeOffsetWrapper> HistorialDeIniciosDeSesion { get; set; } = new List<ModeloDateTimeOffsetWrapper>();

        /// <summary>
        /// Roles del usuario
        /// </summary>
        public virtual List<ModeloRol> Roles { get; set; } = new List<ModeloRol>();

        public ModeloUsuario()
        {
            SecurityStamp = Guid.NewGuid().ToString();
        }
    }
}
