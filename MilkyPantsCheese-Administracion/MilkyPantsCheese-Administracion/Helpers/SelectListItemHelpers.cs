using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MilkyPantsCheese
{
    public static class SelectListItemHelpers
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

        /// <summary>
        /// Obtenemos una lista de <see cref="SelectListItem"/> de la lista de <see cref="T"/> que se pase.
        /// </summary>
        /// <typeparam name="TCisterna"></typeparam>
        /// <param name="tambos"></param>
        /// <returns></returns>
        public static List<SelectListItem> ToSelectListItemListCisterna<TCisterna>(List<TCisterna> cisternas)
            where TCisterna : ModeloCisterna
        {
            return cisternas.Select(t => new SelectListItem( $"{t.Nombre} Lotes: {t.LotesDeLeche.Count}", t.Id.ToString())).ToList();
        }

        /// <summary>
        /// Obtenemos una lista de <see cref="SelectListItem"/> de la lista de <see cref="T"/> que se pase.
        /// </summary>
        /// <typeparam name="TTipoQueso"></typeparam>
        /// <param name="tiposQuesos"></param>
        /// <returns></returns>
        public static List<SelectListItem> ToSelectListItemListTipoQueso<TTipoQueso>(List<TTipoQueso> tiposQuesos)
            where TTipoQueso : ModeloTipoQueso
        {
            return tiposQuesos.Select(t => new SelectListItem( $"{t.Nombre}", t.Id.ToString())).ToList();
        }

        /// <summary>
        /// Obtenemos una lista de <see cref="SelectListItem"/> de la lista de <see cref="T"/> que se pase.
        /// </summary>
        /// <typeparam name="TFermento"></typeparam>
        /// <param name="fermentos"></param>
        /// <returns></returns>
        public static List<SelectListItem> ToSelectListItemListFermento<TFermento>(List<TFermento> fermentos)
            where TFermento : ModeloFermento
        {
            return fermentos.Select(t => new SelectListItem( $"{t.TipoFermento.Nombre} {t.Peso} Kg", t.Id.ToString())).ToList();
        }

        /// <summary>
        /// Obtenemos una lista de <see cref="SelectListItem"/> de la lista de <see cref="T"/> que se pase.
        /// </summary>
        /// <typeparam name="TLoteDeQueso"></typeparam>
        /// <param name="lotes"></param>
        /// <returns></returns>
        public static List<SelectListItem> ToSelectListItemListLotesDeQueso<TLoteDeQueso>(List<TLoteDeQueso> lotes)
            where TLoteDeQueso : ModeloLoteDeQueso
        {
            return lotes.Select(t => new SelectListItem( $"{t.TipoQueso.Nombre} Quesos: {t.Quesos.Count}", t.Id.ToString())).ToList();
        }

        /// <summary>
        /// Obtenemos una lista de <see cref="SelectListItem"/> de la lista de <see cref="T"/> que se pase.
        /// </summary>
        /// <typeparam name="TLoteDeLeche"></typeparam>
        /// <param name="lotes"></param>
        /// <returns></returns>
        public static List<SelectListItem> ToSelectListItemListLotesDeLeche<TLoteDeLeche>(List<TLoteDeLeche> lotes)
            where TLoteDeLeche : ModeloLoteDeLeche
        {
            return lotes.Select(t => new SelectListItem( $"{t.TamboDeProveniencia.Nombre} Lotes de quesos: {t.LotesDeQueso.Count}", t.Id.ToString())).ToList();
        }
    }
}
