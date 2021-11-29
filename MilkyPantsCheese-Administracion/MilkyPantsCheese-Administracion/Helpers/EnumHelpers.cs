using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MilkyPantsCheese.Helpers
{
    /// <summary>
    /// Contiene metodos destinados a facilitar el realizar operaciones con enums
    /// </summary>
    public static class EnumHelpers
    {
        /// <summary>
        /// Crea una <see cref="List{T}"/> de <see cref="SelectListItem"/> con los valores de <typeparamref name="TEnum"/>
        /// </summary>
        /// <typeparam name="TEnum">Tipo del enum del que obtener los valores</typeparam>
        /// <param name="valoresEnum">Parametro opcional con los valores especificos del <typeparamref name="TEnum"/> que meter en la lista</param>
        /// <returns><see cref="List{T}"/> de <see cref="SelectListItem"/></returns>
        public static List<SelectListItem> ToSelectListItemList<TEnum>(List<TEnum> valoresEnum = null)
            where TEnum : struct, Enum
        {
            var valoresConvertir = valoresEnum ?? Enum.GetValues<TEnum>().ToList();

            return valoresConvertir.Select(v => new SelectListItem(v.ToString(), v.ToString())).ToList();
        }
    }
}
