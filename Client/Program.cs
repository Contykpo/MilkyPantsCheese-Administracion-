using System;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
	class Program
	{
		private static TcpClient tcpClient;
		private static NetworkStream stream;

		static void Main(string[] args)
		{
			tcpClient = new TcpClient(args[0], int.Parse(args[1]));

			stream = tcpClient.GetStream();

			while (true)
			{
				ConsoleKey teclaPresionada;

				do
				{
					Console.WriteLine("Presione 'M' para ingresar los valores a mano o 'R' para generarlos al azar");

					teclaPresionada = Console.ReadKey().Key;
				}while(teclaPresionada != ConsoleKey.M && teclaPresionada != ConsoleKey.R);

				decimal temperatura;
				decimal humedad;
				decimal dioxidoDeCarbono;

				if (teclaPresionada == ConsoleKey.M)
				{
					temperatura = PedirIngresoDeDecimal("Ingrese la temperatura", 4, 1);
					humedad = PedirIngresoDeDecimal("Ingrese la humedad", 4, 1);
					dioxidoDeCarbono = PedirIngresoDeDecimal("Ingrese el dioxido de carbono", 10, 5);
				}
				else
				{
					var rand = new Random();

					temperatura = Math.Round((decimal)(rand.NextDouble() * 100), 1);
					humedad = Math.Round((decimal)(rand.NextDouble() * 100), 1);
					dioxidoDeCarbono = Math.Round((decimal)(rand.NextDouble() * 300), 5);
				}

				EnviarMensaje(temperatura, humedad, dioxidoDeCarbono);

				Console.WriteLine("Presione 'X' para salir o cualquier otra tecla para enviar otro mensaje...");

				if(Console.ReadKey().Key == ConsoleKey.X)
					break;
			}

			EnviarMensaje("X");

			EsperarCooler();

			tcpClient.Dispose();
		}

		private static void EnviarMensaje(decimal temperatura, decimal humedad, decimal dioxidoDeCarbono)
		{
			if (tcpClient.Connected)
			{
				EnviarMensaje($"T:{temperatura}-H:{humedad}-D:{dioxidoDeCarbono}");
			}

			while(!stream.DataAvailable)
				Thread.Sleep(100);

			EsperarCooler();
		}

		private static void EsperarCooler()
		{
			var buff = new byte[128];

			int cantBytesLeidos = 0;

			StringBuilder cadenaFinal = new StringBuilder();

			while (true)
			{
				cantBytesLeidos = stream.Read(buff, 0, buff.Length);

				cadenaFinal.Append(Encoding.ASCII.GetString(buff, 0, cantBytesLeidos));

				if (cadenaFinal.ToString().Contains("COOLER", StringComparison.OrdinalIgnoreCase))
					break;
			}
		}

		private static void EnviarMensaje(string mensaje)
		{
			var buff = Encoding.ASCII.GetBytes($"B-{mensaje}-E");

			stream.Write(buff, 0, buff.Length);
		}

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

		private static bool TryParseDecimal(string cadena, int precision, int escala, out decimal resultado)
		{
			var secciones = cadena.Split('.', ',');

			var parteEntera = secciones[0].Substring(0, Math.Min(precision - escala, secciones[0].Length));
			var parteDecimal = secciones[1].Substring(0, Math.Min(escala, secciones[1].Length));

			return decimal.TryParse(
				$"{parteEntera}{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}{parteDecimal}",
				out resultado);
		}
	}

	
}
