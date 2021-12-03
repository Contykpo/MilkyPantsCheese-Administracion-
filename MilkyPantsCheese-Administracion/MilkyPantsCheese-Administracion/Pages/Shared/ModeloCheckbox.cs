using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;

namespace MilkyPantsCheese.Pages
{
	/// <summary>
	/// Modelo para las partial view de checkbox
	/// </summary>
	public record ModeloCheckbox
	{
		/// <summary>
		/// Nombre de la propiedad a la que esta atado el checkbox
		/// </summary>
		public string nombrePropiedad;

		/// <summary>
		/// Contenido de la etiqueta del checkbox
		/// </summary>
		public string contenido;

		/// <summary>
		/// Atributos que añadir al checkbox
		/// </summary>
		public object atributos;

		/// <summary>
		/// Checkbox
		/// </summary>
		public IHtmlContent checkBox;
	}
}
