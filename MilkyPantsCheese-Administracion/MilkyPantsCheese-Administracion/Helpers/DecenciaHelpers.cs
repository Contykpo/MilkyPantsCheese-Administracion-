using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MilkyPantsCheese
{
	/// <summary>
	/// Contiene metodos relacionados a hacer decentes los nombres de los <see cref="ModeloRol"/> de los <see cref="ModeloUsuario"/>
	/// </summary>
	public static class DecenciaHelpers
	{
		/// <summary>
		/// Obtiene una <see cref="List{T}"/> de <see cref="SelectListItem"/> con los tipos de usuario disponibles
		/// </summary>
		/// <returns><see cref="List{T}"/> de <see cref="SelectListItem"/> con los tipos de usuario disponibles</returns>
		public static List<SelectListItem> ObtenerTiposDeUsuarioDisponibles()
		{
			return new List<SelectListItem>
			{
				new SelectListItem("Curador", Constantes.NombreRolCheeseDoctor),
				new SelectListItem("Elaborador", Constantes.NombreRolSusurradorDeQuesos),
				new SelectListItem("Elaborador de fermentos", Constantes.NombreRolCheeseScientist),
				new SelectListItem("Receptor de cargamentos de leche", Constantes.NombreRolMilkyMan),
				new SelectListItem("Trabajador", Constantes.NombreRolMilkyWorker),
				new SelectListItem(Constantes.NombreRolContador, Constantes.NombreRolContador),
				new SelectListItem(Constantes.NombreRolOficinista, Constantes.NombreRolOficinista)
			};
		}

		/// <summary>
		/// Obtiene el nombre 'decente' de un rol
		/// </summary>
		/// <param name="nombreRol">Nombre del rol</param>
		/// <returns>Nombre 'decente' del <paramref name="nombreRol"/> pasado</returns>
		public static string ObtenerNombreDecente(this string nombreRol)
		{
			switch (nombreRol)
			{
				case Constantes.NombreRolCheeseDoctor:
					return "Curador";

				case Constantes.NombreRolSusurradorDeQuesos:
					return "Elaborador";

				case Constantes.NombreRolCheeseScientist:
					return "Elaborador de fermentos";

				case Constantes.NombreRolMilkyMan:
					return "Receptor de cargamentos de leche";

				case Constantes.NombreRolMilkyWorker:
					return "Trabajador";

				default:
					return nombreRol;
			}
		}
	}
}
