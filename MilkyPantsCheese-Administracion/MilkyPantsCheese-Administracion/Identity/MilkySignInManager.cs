using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MilkyPantsCheese
{
	/// <summary>
	/// Implementacion de <see cref="SignInManager{TUser}"/> especializada para las necesidades de esta aplicacion
	/// </summary>
	public class MilkySignInManager : SignInManager<ModeloUsuario>
	{
		public readonly MilkyDbContext _dbContext;

		public MilkySignInManager(
			UserManager<ModeloUsuario> userManager,
			MilkyDbContext dbContext,
			IHttpContextAccessor contextAccessor,
			IUserClaimsPrincipalFactory<ModeloUsuario> claimsFactory,
			IOptions<IdentityOptions> optionsAccessor,
			ILogger<SignInManager<ModeloUsuario>> logger,
			IAuthenticationSchemeProvider schemes,
			IUserConfirmation<ModeloUsuario> confirmation)

			: base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
		{
			_dbContext = dbContext;
		}

		public override async Task<EResultadoLogin> SignInAsync(
			ModeloUsuario usuario,
			AuthenticationProperties propiedadesDeAutenticacion,
			string metodoDeAutenticacion = null)
		{
			//Si el usuario esta deshabilitdo...
			if (!usuario.EstaHabilitado)
				return EResultadoLogin.UsuarioDeshabilitado;

			//Si el usuario esta suspendido...
			if (DateTimeOffset.UtcNow < usuario.FinSuspension)
				return EResultadoLogin.UsuarioSuspendido;

			//Nos logueamos
			await base.SignInAsync(usuario, propiedadesDeAutenticacion, metodoDeAutenticacion);

			//Añadimos este inicio de sesion al historial
			usuario.HistorialDeIniciosDeSesion.Add(new ModeloDateTimeOffsetWrapper{Fecha = DateTimeOffset.UtcNow});

			usuario.TieneSesionAbierta = true;

			await _dbContext.SaveChangesAsync();

			return EResultadoLogin.Exito;
		}

		public override async Task SignOutAsync()
		{
			await base.SignOutAsync();

			var usuarioActual = await UserManager.GetUserAsync(Context.User);
			
			usuarioActual.TieneSesionAbierta = false;

			await _dbContext.SaveChangesAsync();
		}

		/// <summary>
		/// Indica si un <paramref name="usuario"/> esta actualmente logueado
		/// </summary>
		/// <param name="usuario">Usuario que revisar si esta logueado</param>
		/// <returns><see cref="bool"/> indicando si el <paramref name="usuario"/> esta logueado</returns>
		public bool IsSignedIn(ModeloUsuario usuario)
		{
			if (usuario is null)
				throw new ArgumentNullException(nameof(usuario), "Usuario no puede ser null");

			var ultimoInicioDeSesion = usuario.HistorialDeIniciosDeSesion.LastOrDefault();

			if (ultimoInicioDeSesion == null)
				return false;

			//Si no ha expirado la sesion y no se cerro manualmente devolvemos verdadero
			return ultimoInicioDeSesion.Fecha + usuario.DuracionSesion > DateTimeOffset.UtcNow && usuario.TieneSesionAbierta;
		}
	}
}