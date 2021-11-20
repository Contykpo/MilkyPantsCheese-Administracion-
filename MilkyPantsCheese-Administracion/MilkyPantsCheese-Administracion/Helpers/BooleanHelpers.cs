
namespace MilkyPantsCheese
{
    public static class BooleanHelpers
    {
        /// <summary>
        /// Obtiene una respuesta de si o no dependiendo el valor de la condicion.
        /// </summary>
        /// <param name="condicion">Condicion.</param>
        /// <returns>Respuesta de "si" o "no" dependiendo del valor de la <paramref name="condicion"/> pasada </returns>
        public static string ObtenerSiNo(this bool condicion)
        {
            return condicion ? "Si." : "No.";
        }
    }
}
