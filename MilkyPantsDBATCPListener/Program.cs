using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MilkyPantsDBATCPListener
{
	class Program
	{
		private static TcpListener   listener;
		private static TcpClient     cliente;
		private static NetworkStream stream;
		private static ConexionBaseBaseDeDatos conexionBaseDeDatos;

		static void Main(string[] args)
		{
			if (!TryParseArgs(args, out conexionBaseDeDatos, out var ip, out var puerto))
			{
				ConsoleHelpers.WriteLine("Argumentos incorrectos", ConsoleColor.Red);

				return;
			}

			InicializarListener(ip, puerto);
		}

		private static bool TryParseArgs(string[] args, out ConexionBaseBaseDeDatos conexionDb, out IPAddress ip, out int puerto)
		{
			conexionDb = null;
			ip         = null;
			puerto     = 0;

			if (args.Length < 3)
				return false;

			conexionBaseDeDatos = new ConexionBaseBaseDeDatos(args[0]);

			return
				conexionBaseDeDatos.conexionEsValida &&
				IPAddress.TryParse(args[1], out ip) && 
				int.TryParse(args[2], out puerto);
		}

		private static void InicializarListener(IPAddress ip, int puerto)
		{
			listener = new TcpListener(ip, puerto);

			listener.Start();

			EsperarConexion();
		}

		private static void EsperarConexion()
		{
			while (true)
			{
				ConsoleHelpers.WriteLine("Escuchando...", ConsoleColor.Green);

				cliente = listener.AcceptTcpClient();

				stream = cliente.GetStream();

				Bucle();
				
				cliente.Close();
			}
		}

		private static void Bucle()
		{
			if (stream is null)
			{
				ConsoleHelpers.WriteLine($"{nameof(stream)} no puede ser null", ConsoleColor.Red);

				return;
			}

			string cadenaActual = string.Empty;
			string seccionActual = string.Empty;

			try
			{
				byte[] buffer = new byte[512];

				int bytesLeidos = 0;

				StringBuilder strBld = new StringBuilder();

				ModeloDatosSensorCurado datosActualesSensor = new ModeloDatosSensorCurado();

				while (true)
				{
					strBld.Clear();

					while (!stream.DataAvailable)
					{
						Thread.Sleep(100);
					}

					ConsoleHelpers.WriteLine("Leyendo datos...", ConsoleColor.Green);

					while (true)
					{
						bytesLeidos = stream.Read(buffer, 0, buffer.Length);

						cadenaActual = Encoding.ASCII.GetString(buffer, 0, bytesLeidos);

						strBld.Append(cadenaActual);

						ConsoleHelpers.WriteLine(cadenaActual, ConsoleColor.Gray);

						if (cadenaActual.StartsWith('B')) 
							break;
					}

					datosActualesSensor.Fecha = DateTimeOffset.UtcNow;

					cadenaActual = strBld.ToString();

					string[] secciones = cadenaActual.Split('-', ':');

					for (int i = 0; i < secciones.Length; ++i)
					{
						seccionActual = secciones[i].ToUpper();

						if (seccionActual is "E" or "X")
							break;

						switch (seccionActual)
						{
							case "B":
							{
								break;
							}
							case "T":
							{
								ConsoleHelpers.WriteLine("Temperatura: " + secciones[++i], ConsoleColor.Green);

								TryParseDecimal(secciones[i], 4, 1, out var resultado);

								datosActualesSensor.Temperatura = resultado;

								break;
							}
							case "H":
							{
								ConsoleHelpers.WriteLine("Humedad: " + secciones[++i], ConsoleColor.Green);

								TryParseDecimal(secciones[i], 4, 1, out var resultado);

								datosActualesSensor.Humedad = resultado;

								break;
							}
							case "D":
							{
								ConsoleHelpers.WriteLine("Dioxido de carbono: " + secciones[++i], ConsoleColor.Green);

								TryParseDecimal(secciones[i], 4, 1, out var resultado);

								datosActualesSensor.DioxidoDeCarbono = resultado;

								break;
							}
						}
					}

					conexionBaseDeDatos.GuardarDatosSensor(datosActualesSensor);

					var respuesta = Encoding.ASCII.GetBytes("COOLER");

					stream.Write(respuesta, 0, respuesta.Length);

					if (seccionActual == "X")
						break;
				}
			}
			catch (Exception ex)
			{

			}
		}

		private static bool TryParseDecimal(string cadena, int precision, int escala, out decimal resultado)
		{
			var secciones = cadena.Split('.', ',');

			var parteEntera  = secciones[0].Substring(0, Math.Min(precision - escala, secciones[0].Length));
			
			string parteDecimal = string.Empty;

			if(secciones.Length > 1)
				parteDecimal = secciones[1].Substring(0, Math.Min(escala, secciones[1].Length));

			return decimal.TryParse(
				$"{parteEntera}{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}{parteDecimal}",
				out resultado);
		}
	}
}