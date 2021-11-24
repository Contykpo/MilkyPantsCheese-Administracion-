using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace MilkyPantsCheese
{
    public static class ValidationHelpers
    {
        private static readonly Dictionary<string, List<byte[]>> PrefijosExtensiones = new Dictionary<string, List<byte[]>>
        {
            {
                ".png", new List<byte[]>
                {
                    new byte[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A}
                }
            },

            {
                ".jpg", new List<byte[]>
                {
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE0},
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE1},
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE8}
                }
            },

            {
                ".jpeg", new List<byte[]>
                {
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE0},
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE2},
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE3}
                }
            },

            {
                ".bmp", new List<byte[]>
                {
                    new byte[] {0x42, 0x4D}
                }
            },
        };

        public static bool ImagenEsValida(this IFormFile archivo)
        {
            var extensionArchivo = Path.GetExtension(archivo.FileName);

            List<byte[]> headersExtension = PrefijosExtensiones[extensionArchivo.ToLower()];
			
            using (var bReader = new BinaryReader(archivo.OpenReadStream()))
            {
                var header = bReader.ReadBytes(headersExtension.Max(h => h.Length));

                return headersExtension.Any(headerActual => header.Take(headerActual.Length).SequenceEqual(headerActual));
            }
        }

        /// <summary>
        /// Intenta parsear una <paramref name="cadena"/> a un <see cref="decimal"/>
        /// </summary>
        /// <param name="cadena">Cadena que intentar parsear</param>
        /// <param name="precision">Precision del <see cref="decimal"/></param>
        /// <param name="escala">Escala del <see cref="decimal"/></param>
        /// <param name="resultado"><see cref="decimal"/> parseado desde la <paramref name="cadena"/> en caso de tener exito</param>
        /// <returns><see cref="bool"/> indicando si se pudo parsear la <paramref name="cadena"/></returns>
        public static bool TryParseDecimal(this string cadena, int precision, int escala, out decimal resultado)
        {
	        var secciones = cadena.Split(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

	        var parteEntera = secciones[0].Substring(0, Math.Min(precision - escala, secciones[0].Length));

	        string parteDecimal = string.Empty;

	        if (secciones.Length > 1)
		        parteDecimal = secciones[1].Substring(0, Math.Min(escala, secciones[1].Length));

	        return decimal.TryParse(
		        $"{parteEntera}{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}{parteDecimal}",
		        out resultado);
        }
    }
}
