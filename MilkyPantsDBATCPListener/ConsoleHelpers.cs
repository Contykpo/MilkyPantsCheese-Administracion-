using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkyPantsDBATCPListener
{
	public static class ConsoleHelpers
	{
		/// <summary>
		/// Muestra un <paramref name="mensaje"/> en consola con el <paramref name="color"/> especificado y luego
		/// regresa el color a su valor original
		/// </summary>
		/// <param name="mensaje">Mensaje que mostrar</param>
		/// <param name="color">Color que darle al mensaje</param>
		public static void WriteLine(string mensaje, ConsoleColor color)
		{
			var colorAnterior = Console.ForegroundColor;

			Console.ForegroundColor = color;

			Console.WriteLine(mensaje);

			Console.ForegroundColor = colorAnterior;
		}
	}
}
