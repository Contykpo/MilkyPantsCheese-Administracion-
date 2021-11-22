using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MilkyPantsCheese
{
	/// <summary>
	/// Clase que contiene metodos destinados a ayudar con operaciones relacionadas a la manipulacion y formateo de datos
	/// </summary>
	public static class Datahelpers
	{
		/// <summary>
		/// Obtiene una <see cref="List{T}"/> de <see cref="SelectListItem"/> que representa a los <see cref="ModeloTipoFermento"/> disponibles
		/// </summary>
		/// <param name="_dbContext">Data context del que obtener los <see cref="ModeloTipoFermento"/></param>
		/// <returns><see cref="List{T}"/> de <see cref="SelectListItem"/> con los <see cref="ModeloTipoFermento"/> disponibles</returns>
		public static async Task<List<SelectListItem>> ObtenerSelectListDeTiposDeFermentos(this MilkyDbContext _dbContext)
		{
			var fermentosDisponibles = await _dbContext.TiposDeFermento.ToListAsync();

			return fermentosDisponibles.Select(t => new SelectListItem(t.Nombre, t.Id.ToString())).ToList();
		}
	}
}
