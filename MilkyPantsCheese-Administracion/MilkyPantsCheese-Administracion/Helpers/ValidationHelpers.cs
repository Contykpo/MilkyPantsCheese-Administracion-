using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace MilkyPantsCheese
{
    /// <summary>
    /// Clase que contiene metodos destinados a ayudar con la validacion de datos que recibe el servidor
    /// </summary>
    public static class ValidationHelpers
    {
        /// <summary>
        /// Diccionario que contiene los prefijos de los formatos de imagen mas conocidos
        /// </summary>
        private static readonly Dictionary<string, List<byte[]>> PrefijosExtensiones = new Dictionary<string, List<byte[]>>
        {
            //Cabecera de los archivos .png
            {
                ".png", new List<byte[]>
                {
                    new byte[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A}
                }
            },

            //Cabecera de los archivos .jpg
            {
                ".jpg", new List<byte[]>
                {
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE0},
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE1},
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE8}
                }
            },

            //Cabecera de los archivos .jpeg
            {
                ".jpeg", new List<byte[]>
                {
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE0},
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE2},
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE3}
                }
            },

            //Cabecera de los archivos .bmp
            {
                ".bmp", new List<byte[]>
                {
                    new byte[] {0x42, 0x4D}
                }
            },
        };

        /// <summary>
        /// Determina si un <paramref name="archivo"/> es valido
        /// </summary>
        /// <param name="archivo">Archivo cuya validez verificar</param>
        /// <returns><see cref="bool"/> indicando si la imagen es valida</returns>
        public static bool ImagenEsValida(this IFormFile archivo)
        {
            var extensionArchivo = Path.GetExtension(archivo.FileName).ToLower();

            if (!PrefijosExtensiones.ContainsKey(extensionArchivo))
	            return false;

            List<byte[]> headersExtension = PrefijosExtensiones[extensionArchivo.ToLower()];
            
            //Leemos la cabecera del archivo y nos aseguramos de que corresponda a su extension
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
        /// <param name="separadorDecimal">Caracter que separa la parte entera de la decimal. Si no se especifica, se utilizara el
        /// especificado por la cultura actual</param>
        /// <returns><see cref="bool"/> indicando si se pudo parsear la <paramref name="cadena"/></returns>
        public static bool TryParseDecimal(this string cadena, int precision, int escala, out decimal resultado, string separadorDecimal = "")
        {
	        resultado = 0;

	        if (string.IsNullOrWhiteSpace(cadena))
		        return false;

            //Si no se especifico el separador entonces tomamos el de la cultura actual
            if(string.IsNullOrWhiteSpace(separadorDecimal))
                separadorDecimal = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            //Separamos el numero en parte entera y decimal
            var secciones = cadena.Split(separadorDecimal);

            //Obtenemos la parte entera del numero y limitamos su longitud de acuerdo a la precision
	        var parteEntera = secciones[0].Substring(0, Math.Min(precision - escala, secciones[0].Length));

	        string parteDecimal = string.Empty;

            //Si tiene parte decimal, la obtenemos tambien y limitamos su longitud de acuerdo a la precision
	        if (secciones.Length > 1)
		        parteDecimal = secciones[1].Substring(0, Math.Min(escala, secciones[1].Length));

	        return decimal.TryParse(
		        $"{parteEntera}{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}{parteDecimal}",
                NumberStyles.AllowDecimalPoint,
                null,
		        out resultado);
        }
    }
}
