namespace MilkyPantsCheese
{
	/// <summary>
	/// Indica el resultado de un intento de login
	/// </summary>
	public enum EResultadoLogin
	{
		/// <summary>
		/// Indica que el inicio de sesion se realizo con exito
		/// </summary>
		Exito = 0,

		/// <summary>
		/// Indica que el inicio de sesion fallo porque el usuario esta deshabilitado
		/// </summary>
		UsuarioDeshabilitado = 1,

		/// <summary>
		/// Indica que el inicio de sesion fallo porque el usuario esta suspendido
		/// </summary>
		UsuarioSuspendido = 2
	}
}
