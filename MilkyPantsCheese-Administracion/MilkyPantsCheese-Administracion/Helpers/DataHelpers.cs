using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

		/// <summary>
		///		<para>
		///			Valida una <paramref name="id"/> y obtiene el <typeparamref name="TModelo"/> al que pertenece
		///		</para>
		///		<para>
		///			Añade los errores encontrados al <paramref name="modelState"/> especificado
		///		</para>
		/// </summary>
		/// <typeparam name="TModelo">Tipo del <see cref="ModeloBase"/> al que pertenece la id</typeparam>
		/// <param name="dbSet"><see cref="DbSet{TEntity}"/> que contiene al <typeparamref name="TModelo"/></param>
		/// <param name="id">Id del <typeparamref name="TModelo"/></param>
		/// <param name="modelState"><see cref="ModelStateDictionary"/> del <see cref="PageModel"/> en el que estamos</param>
		/// <param name="nombrePropiedad">Nombre de la propiedad a la que asignar los errores</param>
		public static async Task<TModelo> ValidarIDYObtenerModelo<TModelo>(
			this DbSet<TModelo> dbSet,
			int id,
			ModelStateDictionary modelState,
			string nombrePropiedad)

			where TModelo : ModeloBase
		{
			//Una id valida nunca puede ser menor a 1
			if (id < 1)
			{
				modelState?.AddModelError(nombrePropiedad, "La id especificada no es valida. Si el problema persiste, contactese con soporte");

				return null;
			}

			//Intentamos encontrar el modelo con la id especificada
			var modeloEncontrado = await dbSet.FirstOrDefaultAsync(m => m.Id == id);

			if (modeloEncontrado is null)
			{
				modelState?.AddModelError(nombrePropiedad, "No se encontro un modelo con la id especificada. Si el problema persistem, contactese con soporte");

				return null;
			}

			return modeloEncontrado;
		}

		/// <summary>
		///		<para>
		///			Intenta guardar los cambios en el <paramref name="dbContext"/>
		///		</para>
		///		<para>
		///			Añade los errores encontrados al <paramref name="modelState"/> especificado
		///		</para>
		/// </summary>
		/// <param name="dbContext"><see cref="MilkyDbContext"/> que intentar guardar</param>
		/// <param name="logger"><see cref="ILogger{TCategoryName}"/> que utlizar para mostrar mensajes</param>
		/// <param name="modelState"><see cref="ModelStateDictionary"/> del <see cref="PageModel"/> en el que estamos</param>
		/// <param name="nombrePropiedad">Nombre de la propiedad a la que asignar los errores</param>
		/// <param name="accionQueRealizarAntes">Delegado que ejecutar antes de guardar los datos</param>
		/// <returns><see cref="bool"/> indicando si se pudieron guardar los datos</returns>
		public static async Task<bool> IntentarGuardarCambios(
			this MilkyDbContext dbContext,
			ILogger logger,
			ModelStateDictionary modelState,
			string nombrePropiedad,
			Action accionQueRealizarAntes = null)
		{
			try
			{
				accionQueRealizarAntes?.Invoke();

				await dbContext.SaveChangesAsync();

				return true;
			}
			catch (Exception ex)
			{
				logger?.LogError(ex, "Ocurrio un error al intentar guardar los cambios");

				modelState?.AddModelError(nombrePropiedad, "Ocurrio un error al intentar guardar los datos. Si el problema persiste, contese con soporte");

				return false;
			}
		}

		/// <summary>
		/// Añade una operacion Where a la <paramref name="consultaActual"/> que ejecuta una de las comparaciones
		/// especificadas en base al <paramref name="modoComparacion"/>
		/// </summary>
		/// <typeparam name="TModelo">Tipo del <see cref="ModeloBase"/> que devuelve la <paramref name="consultaActual"/></typeparam>
		/// <param name="consultaActual"><see cref="IQueryable{TModelo}"/> a la que añadir el Where</param>
		/// <param name="comparacionExacto">Delegado a ejecutar en caso de que se deba realizar una comparacion exacta</param>
		/// <param name="comparacionMayor">Delegado a ejecutar en caso de que se deba realizar una comparacion mayor a</param>
		/// <param name="comparacionMenor">Delegado a ejecutar en caso de que se deba realizar una comparacion menor a</param>
		/// <param name="modoComparacion">Comparacion que se realizara</param>
		/// <returns><see cref="IQueryable{TModelo}"/> con la operacion Where añadida</returns>
		public static IQueryable<TModelo> AñadirWhereConTipoDeComparacion<TModelo>(
			this IQueryable<TModelo> consultaActual,
			Expression<Func<TModelo, bool>> comparacionExacto,
			Expression<Func<TModelo, bool>> comparacionMayor,
			Expression<Func<TModelo, bool>> comparacionMenor,
			EModoComparacion modoComparacion)

			where TModelo : ModeloBase
		{
			switch (modoComparacion)
			{
				case EModoComparacion.Exacto:
					return consultaActual.Where(comparacionExacto);
				case EModoComparacion.Mayor:
					return consultaActual.Where(comparacionMayor);
				case EModoComparacion.Menor:
					return consultaActual.Where(comparacionMenor);
			}

			return consultaActual;
		}
	}
}