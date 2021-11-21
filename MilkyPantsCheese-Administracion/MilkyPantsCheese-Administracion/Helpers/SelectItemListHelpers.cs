using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MilkyPantsCheese
{
    public static class SelectItemListHelpers
    {
        /// <summary>
        /// Obtenemos una lista de <see cref="SelectListItem"/> de la lista de <see cref="T"/> que se pase.
        /// </summary>
        /// <typeparam name="TTambo"></typeparam>
        /// <param name="tambos"></param>
        /// <returns></returns>
        public static List<SelectListItem> ToSelectListItemListTambos<TTambo>(List<TTambo> tambos)
            where TTambo : ModeloTambo
        {
            return tambos.Select(t => new SelectListItem( $"{t.Nombre} Lotes: {t.LotesDeLecheDeEsteTambo.Count}", t.Id.ToString())).ToList();
        }
    }
}
