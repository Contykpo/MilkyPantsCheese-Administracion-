﻿using System;
using System.ComponentModel.DataAnnotations;

namespace MilkyPantsCheese
{
    /// <summary>
    /// Modelo que representa a un queso.
    /// </summary>
    public class ModeloQueso : ModeloBase
    {
        /// <summary>
        /// Estado actual del queso.
        /// </summary>
        public EEstadoQueso EstadoQueso { get; set; }

        /// <summary>
        /// Pero del queso antes de comenzar el curado del mismo.
        /// </summary>
        public decimal PesoPreCurado { get; set; }

        /// <summary>
        /// Peso del queso tras finalizar el curado del mismo.
        /// </summary>
        public decimal PesoPostCurado { get; set; }

        /// <summary>
        /// Fecha en la que finalizo la curacion de este queso
        /// </summary>
        public DateTimeOffset FechaFinCuracion { get; set; }

        /// <summary>
        /// Lote que compone este queso.
        /// </summary>
        public virtual ModeloLoteDeQueso Lote { get; set; }
    }
}
