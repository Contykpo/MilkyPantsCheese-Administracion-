using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MilkyPantsCheese
{
	/// <summary>
	/// Implementacion de <see cref="UserManager{TUser}"/> para esta aplicacion
	/// </summary>
	public class MilkyUserManager : UserManager<ModeloUsuario>
	{
		public readonly MilkyDbContext _dbContext;

		public MilkyUserManager(
			MilkyDbContext dbContext,
			IUserStore<ModeloUsuario> store,
			IOptions<IdentityOptions> optionsAccessor,
			IPasswordHasher<ModeloUsuario> passwordHasher,
			IEnumerable<IUserValidator<ModeloUsuario>> userValidators,
			IEnumerable<IPasswordValidator<ModeloUsuario>> passwordValidators,
			ILookupNormalizer keyNormalizer,
			IdentityErrorDescriber errors,
			IServiceProvider services,
			ILogger<UserManager<ModeloUsuario>> logger)

			: base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors,
				services, logger)
		{
			_dbContext = dbContext;
		}

		/// <summary>
		/// Añade un <paramref name="usuario"/> a un <paramref name="rol"/> especificado
		/// </summary>
		/// <param name="usuario"><see cref="ModeloUsuario"/> que añadir al <paramref name="rol"/></param>
		/// <param name="rol">Nombre del <see cref="ModeloRol"/> al que añadir al <paramref name="usuario"/></param>
		public override async Task<IdentityResult> AddToRoleAsync(ModeloUsuario usuario, string rol)
		{
			var resultado = await base.AddToRoleAsync(usuario, rol);

			if(resultado != IdentityResult.Success)
				return resultado;

			var rolEncontrado = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == rol);

			usuario.Roles.Add(rolEncontrado);
			rolEncontrado.Usuarios.Add(usuario);

			return resultado;
		}

		/// <summary>
		/// Quita un <paramref name="usuario"/> de un <paramref name="rol"/>
		/// </summary>
		/// <param name="usuario"><see cref="ModeloUsuario"/> que quitar del <paramref name="rol"/></param>
		/// <param name="rol">Nombre del <see cref="ModeloRol"/> del que quitar al <paramref name="usuario"/></param>
		public override async Task<IdentityResult> RemoveFromRoleAsync(ModeloUsuario usuario, string rol)
		{
			var resultado = await base.RemoveFromRoleAsync(usuario, rol);

			if (resultado != IdentityResult.Success)
				return resultado;

			var rolEncontrado = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == rol);

			usuario.Roles.Remove(rolEncontrado);
			rolEncontrado.Usuarios.Remove(usuario);

			return resultado;
		}

		/// <summary>
		/// Obtiene el <see cref="ModeloUsuario"/> con la <paramref name="id"/> especificada asincronicamente
		/// </summary>
		/// <param name="id">Id del <see cref="ModeloUsuario"/></param>
		/// <returns><see cref="ModeloUsuario"/> encontrado o null</returns>
		public async Task<ModeloUsuario> GetUserByIdAsync(int id)
		{
			if (id <= 0)
				return null;

			return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
		}
	}
}
