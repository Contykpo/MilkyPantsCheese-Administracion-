using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MilkyPantsCheese.Pages
{
	[Authorize(Roles = Constantes.NombreRolAdministrador)]
    public class AdministrarUsuariosModel : PageModel
    {
	    public readonly MilkyDbContext _dbContext;

	    public AdministrarUsuariosModel(MilkyDbContext dbContext)
	    {
		    _dbContext = dbContext;
	    }

		#region Parametros buscador

		[BindProperty]
		[DisplayName("Mostrar usuarios deshabilitados")]
		public bool MostrarUsuariosDeshabilitados { get; set; } = true;

		[BindProperty]
		[DisplayName("Nombre del usuario")]
		public string NombreUsuario { get; set; }

		[BindProperty]
		[DisplayName("Tipos de usuario que mostrar")]
		public string[] TiposDeUsuarioSeleccionados { get; set; }

		public List<ModeloUsuario> UsuariosFiltrados { get; set; } = new List<ModeloUsuario>();

		public List<SelectListItem> TiposDeUsuarioDisponibles { get; set; } = DecenciaHelpers.ObtenerTiposDeUsuarioDisponibles();

		#endregion

		#region Parametros suspension

		[BindProperty]
		[DisplayName("Fecha en la que finaliza la suspension")]
		public DateTimeOffset FechaFinSuspension { get; set; }

		#endregion

		public void OnGet()
        {
        }

	    public async Task<IActionResult> OnPost()
	    {
		    IQueryable<ModeloUsuario> query = _dbContext.Users.AsQueryable();
		    
		    if (!string.IsNullOrWhiteSpace(NombreUsuario))
			    query = query.Where(u => u.UserName == NombreUsuario);

		    if (!MostrarUsuariosDeshabilitados)
			    query = query.Where(u => u.EstaHabilitado);

		    if (TiposDeUsuarioSeleccionados.Length > 0)
			    query = query.Where(u => u.Roles.Any(r => TiposDeUsuarioSeleccionados.Contains(r.Name)));

		    UsuariosFiltrados = await query.ToListAsync();

		    return Partial("_ListaUsuarios", this);
	    }

		/// <summary>
		/// Cambia la fecha del fin de la suspension de un <see cref="ModeloUsuario"/>
		/// </summary>
		/// <param name="id">Id del <see cref="ModeloUsuario"/></param>
		public async Task OnPutSuspender([FromQuery(Name = "id")]int id)
	    {
		    var usuarioEncontrado = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

		    usuarioEncontrado.FinSuspension = FechaFinSuspension;

		    await _dbContext.SaveChangesAsync();
	    }

		/// <summary>
		/// Habilita un deshabilita un <see cref="ModeloUsuario"/> con el <paramref name="id"/> especificado
		/// </summary>
		/// <param name="id">Id del usuario</param>
		public async Task OnPutToggleUsuarioDeshabilitado([FromQuery(Name = "id")] int id)
		{
			var usuarioEncontrado = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

			usuarioEncontrado.EstaHabilitado = !usuarioEncontrado.EstaHabilitado;

			await _dbContext.SaveChangesAsync();
		}
    }
}