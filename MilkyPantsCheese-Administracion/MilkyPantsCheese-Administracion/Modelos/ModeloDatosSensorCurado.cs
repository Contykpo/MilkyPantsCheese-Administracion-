using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilkyPantsCheese
{
	/// <summary>
	/// Modelo que representa los datos recibidos por el sensor del sector de curado
	/// </summary>
	public class ModeloDatosSensorCurado : ModeloBase
	{
		/// <summary>
		/// Temperatura del ambiente, en grados celsius
		/// </summary>
		public decimal Temperatura { get; set; }

		/// <summary>
		/// Humedad del ambiente
		/// </summary>
		public decimal Humedad { get; set; }

		/// <summary>
		/// Dioxido de carbono, en partes por millon
		/// </summary>
		public decimal DioxidoDeCarbono { get; set; }

		/// <summary>
		/// Fecha, hora y minuto de estos datos
		/// </summary>
		public DateTimeOffset Fecha { get; set; }
	}
}
