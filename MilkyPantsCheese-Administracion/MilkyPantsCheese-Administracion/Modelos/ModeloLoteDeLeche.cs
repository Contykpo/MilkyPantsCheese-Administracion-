using System;
using System.ComponentModel.DataAnnotations;
using MilkyPantsCheese_Administracion.Modelos;

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
		public int Acidez { get; set; }

		/// <summary>
		/// Indica si este lote esta disponible para ser utilizado
		/// </summary>
		public bool EstaDisponible { get; set; }

		/// <summary>
		/// Imagen de la planilla de este lote
		/// </summary>
		[MaxLength(20971520)]
		public byte[] ImagenPlanilla { get; set; }

		/// <summary>
		/// Notas adicionales sobre este lote
		/// </summary>
		[StringLength(1024)]
		public string NotasAdicionales { get; set; }

		/// <summary>
		/// Fecha y hora de llegada de este lote
		/// </summary>
		public DateTimeOffset Fecha { get; set; }

	}
}
