using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkyPantsCheese
{
	/// <summary>
	/// Modelo que representa un lote de leche
	/// </summary>
	public class ModeloLoteDeLeche : ModeloBase
	{
		/// <summary>
		/// Porcentaje de agua
		/// </summary>
		public decimal PorcentajeDeAgua { get; set; }

		/// <summary>
		/// Temperatura con la que llego la leche
		/// </summary>
		public decimal Temperatura { get; set; }

		/// <summary>
		/// Acidez de la leche, medida en grados Dornic
		/// </summary>
		public decimal Acidez { get; set; }

		/// <summary>
		/// Indica si este lote esta disponible para ser utilizado
		/// </summary>
		public bool EstaDisponible { get; set; }

		/// <summary>
		/// Imagen de la planilla de este lote
		/// </summary>
        [MaxLength(2097062)]
		public byte[] ImagenPlanilla { get; set; }

		/// <summary>
		/// Notas adicionales sobre este lote
		/// </summary>
		[Column(TypeName = "nvarchar(1024)")]
		public string NotasAdicionales { get; set; }

		/// <summary>
		/// Fecha y hora de llegada de este lote
		/// </summary>
		public DateTimeOffset Fecha { get; set; }

		/// <summary>
		/// Tambo del que proviene este lote de leche.
		/// </summary>
		public virtual ModeloTambo TamboDeProveniencia { get; set; }

		/// <summary>
		/// Cisterna donde este lote de leche es almacenado.
		/// </summary>
		public virtual ModeloCisterna Cisterna { get; set; }

        /// <summary>
        /// Lotes de queso que compone este lote.
        /// </summary>
        public virtual List<ModeloLoteDeQueso> LotesDeQueso { get; set; } = new List<ModeloLoteDeQueso>();
	}
}
