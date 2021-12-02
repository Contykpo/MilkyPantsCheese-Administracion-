using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
	class Program
	{
		/// <summary>
		/// URL de la pagina a la que enviamos los datos
		/// </summary>
		static private string urlPagina = string.Empty;

		static async Task<int> Main(string[] args)
		{
			//Validamos los argumentos
			if (!await ValidarArgumentos(args))
				return -1;

			//Si la validacion tuvo exito asignamos la url a la pasada en los parametros
			urlPagina = args[0];

			//Continuamos iterando hasta que el usuario presione la tecla 'X' para salir
			while (true)
			{
				ConsoleKey teclaPresionada;

				//Iteramos mientras que la tecla presionada por el usuario no sea una de las pedidas
				do
				{
					Console.WriteLine("Presione 'M' para ingresar los valores a mano o 'R' para generarlos al azar");

					teclaPresionada = Console.ReadKey().Key;
				} while (teclaPresionada != ConsoleKey.M && teclaPresionada != ConsoleKey.R);

				decimal temperatura;
				decimal humedad;
				decimal dioxidoDeCarbono;

				//Si el usuario quiere ingresar los datos manualmente...
				if (teclaPresionada == ConsoleKey.M)
				{
					temperatura      = PedirIngresoDeDecimal("Ingrese la temperatura", 5, 2);
					humedad          = PedirIngresoDeDecimal("Ingrese la humedad", 5, 2);
					dioxidoDeCarbono = PedirIngresoDeDecimal("Ingrese el dioxido de carbono", 10, 5);
				}
				//Si el usuario quiere que los datos se generen al azar...
				else
				{
					var rand = new Random();

					temperatura      = Math.Round((decimal)(rand.NextDouble() * 100), 2);
					humedad          = Math.Round((decimal)(rand.NextDouble() * 100), 2);
					dioxidoDeCarbono = Math.Round((decimal)(rand.NextDouble() * 9999), 5);
				}

				//Enviamos la solicitud al servidor
				await EnviarDatos(temperatura, humedad, dioxidoDeCarbono);

				Console.WriteLine("Presione 'X' para salir o cualquier otra tecla para enviar otro mensaje...");

				if (Console.ReadKey().Key == ConsoleKey.X)
					break;
			}

			return 0;
		}

		/// <summary>
		/// Envia un post al api del servidor para actualizar los datos del sensor
		/// </summary>
		/// <param name="temperatura">Valor de temperatura</param>
		/// <param name="humedad">Valor de humedad</param>
		/// <param name="dioxidoDeCarbono">Valor de dioxido de carbono</param>
		private static async Task EnviarDatos(decimal temperatura, decimal humedad, decimal dioxidoDeCarbono)
		{
			try
			{
				//Creamos el cliente http
				using HttpClient clienteHttp = new HttpClient();

				//Enviamos la solicitud de post con los datos pasados
				await clienteHttp.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"{urlPagina}/api/sensorcurado?t={temperatura}&h={humedad}&d={dioxidoDeCarbono}"));
			}
			catch (Exception ex)
			{
				WriteLine(ConsoleColor.DarkRed, $"Error al enviar datos {ex.Message}");
			}
		}

		/// <summary>
		/// <para>
		///		Muestra en pantalla un mensaje solicitando el ingreso de un numero decimal e intenta parsear el numero ingresado
		/// </para>
		/// <para>
		///		Este proceso se repite hasta que el usuario ingrese un numero decimal valido
		/// </para>
		/// </summary>
		/// <param name="mensaje">Mensaje que mostrar</param>
		/// <param name="precision">Preciosion del numero</param>
		/// <param name="escala">Escala del numero</param>
		/// <returns><see cref="decimal"/> ingresado por el usuario con la <paramref name="precision"/> y <paramref name="escala"/> especificados</returns>
		private static decimal PedirIngresoDeDecimal(string mensaje, int precision, int escala)
		{
			decimal resultado;
			string cadenaIngresada;

			do
			{
				Console.WriteLine(mensaje);

				cadenaIngresada = Console.ReadLine();

			} while (!TryParseDecimal(cadenaIngresada, precision, escala, out resultado));

			return resultado;
		}

		/// <summary>
		/// Intenta parsear una <paramref name="cadena"/> a un <see cref="decimal"/>
		/// </summary>
		/// <param name="cadena">Cadena que intentar parsear</param>
		/// <param name="precision">Preciosion que tendra el <paramref name="resultado"/></param>
		/// <param name="escala">Escala que tendra el <paramref name="resultado"/></param>
		/// <param name="resultado">Resultado del parseo</param>
		/// <returns><see cref="bool"/> indicando si se pudo parsear la <paramref name="cadena"/></returns>
		private static bool TryParseDecimal(string cadena, int precision, int escala, out decimal resultado)
		{
			string parteDecimal = "0";

			//Dividimos el numero en sus partes entera y decimal
			var secciones = cadena.Split('.', ',');

			//Limitamos el tamaño de la parte entera
			var parteEntera  = secciones[0].Substring(0, Math.Min(precision - escala, secciones[0].Length));

			//Si hay parte decimal tambien limitamos su tamaño
			if(secciones.Length > 1)
				parteDecimal = secciones[1].Substring(0, Math.Min(escala, secciones[1].Length));

			//Intentamos parsear el numero y devolvemos el resultado
			return decimal.TryParse(
				$"{parteEntera}.{parteDecimal}",
				out resultado);
		}

		/// <summary>
		/// Valida los argumentos pasados al iniciar la aplicacion
		/// </summary>
		/// <param name="args">Argumentos pasados a la aplicacion</param>
		/// <returns><see cref="bool"/> indicando si los argumentos son validos</returns>
		private static async Task<bool> ValidarArgumentos(string[] args)
		{
			if (args.Length == 0)
			{
				WriteLine(ConsoleColor.DarkRed, "Argumentos insuficientes");

				return false;
			}

			try
			{
				//Intentamos realizar una solicitud get al servidor urilizando la url pasada
				using var cliente = new HttpClient();

				await cliente.SendAsync(new HttpRequestMessage(HttpMethod.Get, args[0]));

				return true;
			}
			catch (Exception ex)
			{
				//Si llegamos aqui entonces sabemos que la url no es valida
				WriteLine(ConsoleColor.Red, "URL provista invalida " + args[0]);

				return false;
			}
		}

		/// <summary>
		/// Helper para mostrar un mensaje en consola con un color especifico
		/// </summary>
		/// <param name="color">Color que darle al mensaje</param>
		/// <param name="mensaje">Mensaje que mostrar</param>
		private static void WriteLine(ConsoleColor color, string mensaje)
		{
			//Color actual de la consola
			var colorPrevio = Console.ForegroundColor;

			Console.ForegroundColor = color;

			Console.WriteLine(mensaje);

			//Luego de mostrar el mensaje restauramos el color anterior de la consola
			Console.ForegroundColor = colorPrevio;
		}
	}
}