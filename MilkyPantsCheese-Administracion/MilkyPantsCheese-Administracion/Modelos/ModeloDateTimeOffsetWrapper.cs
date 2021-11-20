using System;

namespace MilkyPantsCheese
{
	/// <summary>
	/// Modelo que representa a un <see cref="DateTimeOffset"/> en una lista dentro de un modelo
	/// </summary>
	public class ModeloDateTimeOffsetWrapper : ModeloBase
	{
		/// <summary>
		/// <see cref="DateTimeOffset"/> contenido por este modelo
		/// </summary>
		public DateTimeOffset Fecha { get; set; }
	}
}
