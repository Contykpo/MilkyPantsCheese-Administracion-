using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MilkyPantsCheese.Pages
{
	/// <summary>
	/// Modelo de la pagina de aviso de cuenta suspendida
	/// </summary>
    public class AvisoCuentaSuspendidaModel : PageModel
    {
	    public readonly MilkyDbContext _dbContext;

	    public AvisoCuentaSuspendidaModel(MilkyDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		/// <summary>
		/// <see cref="ModeloUsuario"/> que se encuentra suspendido
		/// </summary>
	    public ModeloUsuario usuario;

		/// <summary>
		/// Obtiene el <see cref="ModeloUsuario"/> suspendido y lo guarda en <see cref="usuario"/>
		/// </summary>
		/// <param name="idUsuario">Id del <see cref="ModeloUsuario"/> que esta suspendido</param>
		public async Task OnGet([FromQuery(Name = "id")]int idUsuario)
	    {
		    usuario = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == idUsuario);
	    }
    }
}
