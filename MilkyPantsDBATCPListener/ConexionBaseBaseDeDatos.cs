using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkyPantsDBATCPListener
{
	public class ConexionBaseBaseDeDatos
	{
		public readonly bool conexionEsValida;
		public readonly string connectionString;

		public ConexionBaseBaseDeDatos(string connString)
		{
			connectionString = connString;
			
			try
			{
				using var conexion = new SqlConnection(connString);

				conexion.Open();
				conexion.Close();

				conexionEsValida = true;
			}
			catch (Exception ex)
			{
				conexionEsValida = false;
			}
		}

		public void GuardarDatosSensor(ModeloDatosSensorCurado datos)
		{
			try
			{
				using (var conexion = new SqlConnection(connectionString))
				{
					SqlCommand comando = new SqlCommand(
						$"INSERT INTO DatosSensorCurado({nameof(ModeloDatosSensorCurado.Temperatura)}, {nameof(ModeloDatosSensorCurado.Humedad)}, {nameof(ModeloDatosSensorCurado.DioxidoDeCarbono)}, {nameof(ModeloDatosSensorCurado.Fecha)}) VALUES(@temperatura, @humedad, @dioxidoDeCarbono, @fecha)");

					comando.Parameters.Add(new SqlParameter
					{
						ParameterName = "@temperatura",
						SqlDbType = SqlDbType.Decimal,
						Precision = 4,
						Scale = 1,

						Value = datos.Temperatura
					});

					comando.Parameters.Add(new SqlParameter
					{
						ParameterName = "@humedad",
						SqlDbType = SqlDbType.Decimal,
						Precision = 4,
						Scale = 1,

						Value = datos.Humedad
					});

					comando.Parameters.Add(new SqlParameter
					{
						ParameterName = "@dioxidoDeCarbono",
						SqlDbType = SqlDbType.Decimal,
						Precision = 10,
						Scale = 5,

						Value = datos.DioxidoDeCarbono
					});

					comando.Parameters.Add(new SqlParameter
					{
						ParameterName = "@fecha",
						SqlDbType = SqlDbType.DateTimeOffset,

						Value = DateTimeOffset.UtcNow
					});

					comando.Connection = conexion;

					conexion.Open();

					comando.ExecuteNonQuery();

					conexion.Close();
				}
			}
			catch (Exception ex)
			{

			}
		}
	}
}
