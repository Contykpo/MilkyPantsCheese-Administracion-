using System.Collections.Generic;
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
    }
}
