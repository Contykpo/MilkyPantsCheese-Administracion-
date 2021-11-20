using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MilkyPantsCheese_Administracion.Modelos;

namespace MilkyPantsCheese
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
	        services.AddAuthentication();

	        var connString = Configuration.GetConnectionString("DefaultConnection");
            
            services.AddDbContext<MilkyDbContext>(config =>
	            config.UseSqlServer(connString));

            services.AddDefaultIdentity<ModeloUsuario>()
	            .AddRoles<ModeloRol>()
	            .AddEntityFrameworkStores<MilkyDbContext>();

            //Configuramos el servicio de autenticacion
            services.Configure<IdentityOptions>(configIdentity =>
            {
	            //Configuracion de los requisitos de la contraseña
	            configIdentity.Password = new PasswordOptions
	            {
		            RequireLowercase = true,
		            RequireUppercase = true,
		            RequireDigit = false,
                    RequireNonAlphanumeric = false,
		            RequiredLength = 4,
		            RequiredUniqueChars = 3
	            };

	            //Configuracion de los requisitos para loguearse
	            configIdentity.SignIn = new SignInOptions
	            {
		            RequireConfirmedAccount = false,
		            RequireConfirmedEmail = false
	            };

	            //Configuracion de las restricciones para crearse un usuario
	            configIdentity.User = new UserOptions
	            {
		            AllowedUserNameCharacters = "abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ0123456789@. ",
		            RequireUniqueEmail = false
	            };

	            //Configuracion de la resctriccion en caso de muchos intentos de logueo fallidos
	            configIdentity.Lockout = new LockoutOptions
	            {
		            MaxFailedAccessAttempts = int.MaxValue,
		            DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1)
	            };
            });

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider servicios)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            //Nos aseguramos de que la base de datos exista y las migraciones esten aplicadas
            CrearBaseDeDatos(servicios).Wait();

            //Nos aseguramos de que todos los roles esten creados
            CrearRolesSiNoExisten((RoleManager<ModeloRol>)servicios.GetService(typeof(RoleManager<ModeloRol>))).Wait();

            //Creamos el usuario del administrador en caso de que no exista
            CrearUsuarioAdministrador(servicios).Wait();
        }

        /// <summary>
        /// Crea la base de datos y aplica las migraciones en caso de ser necesario
        /// </summary>
        /// <param name="servicios">Proveedor de servicios que contiene el <see cref="MilkyDbContext"/></param>
        private async Task CrearBaseDeDatos(IServiceProvider servicios)
        {
            await using var dbContext = servicios.CreateScope().ServiceProvider.GetRequiredService<MilkyDbContext>();

            await dbContext.Database.MigrateAsync();
        }

        /// <summary>
        /// Crea los roles utilizados por la aplicacion en caso de que no existan
        /// </summary>
        /// <param name="roleManager">Administrador de roles</param>
        private async Task CrearRolesSiNoExisten(RoleManager<ModeloRol> roleManager)
        {
            //Rol de administrador
	        if (!await roleManager.RoleExistsAsync(Constantes.NombreRolAdministrador))
		        await roleManager.CreateAsync(new ModeloRol(Constantes.NombreRolAdministrador));

            //Rol de curador
	        if (!await roleManager.RoleExistsAsync(Constantes.NombreRolCheeseDoctor))
		        await roleManager.CreateAsync(new ModeloRol(Constantes.NombreRolCheeseDoctor));

            //Rol señor de los fermentados
	        if (!await roleManager.RoleExistsAsync(Constantes.NombreRolCheeseScientist))
		        await roleManager.CreateAsync(new ModeloRol(Constantes.NombreRolCheeseScientist));

            //Rol contador
	        if (!await roleManager.RoleExistsAsync(Constantes.NombreRolContador))
		        await roleManager.CreateAsync(new ModeloRol(Constantes.NombreRolContador));

            //Rol receptor de leche
	        if (!await roleManager.RoleExistsAsync(Constantes.NombreRolMilkyMan))
		        await roleManager.CreateAsync(new ModeloRol(Constantes.NombreRolMilkyMan));

            //Rol trabajador no especificado
	        if (!await roleManager.RoleExistsAsync(Constantes.NombreRolMilkyWorker))
		        await roleManager.CreateAsync(new ModeloRol(Constantes.NombreRolMilkyWorker));

            //Rol oficinista
	        if (!await roleManager.RoleExistsAsync(Constantes.NombreRolOficinista))
		        await roleManager.CreateAsync(new ModeloRol(Constantes.NombreRolOficinista));

            //Rol productor lote de queso
	        if (!await roleManager.RoleExistsAsync(Constantes.NombreRolSusurradorDeQuesos))
		        await roleManager.CreateAsync(new ModeloRol(Constantes.NombreRolSusurradorDeQuesos));
        }

        /// <summary>
        /// Crea al usuario administrador en caso de que no exista
        /// </summary>
        /// <param name="servicios">Proveedor de servicios que contiene el <see cref="MilkyDbContext"/> y el <see cref="UserManager{TUser}"/></param>
        private async Task CrearUsuarioAdministrador(IServiceProvider servicios)
        {
	        var userManager = servicios.GetRequiredService<UserManager<ModeloUsuario>>();

	        await using var dbContext = servicios.CreateScope().ServiceProvider.GetRequiredService<MilkyDbContext>();

	        var usuarios = dbContext.Users.ToList();

            if(usuarios.FirstOrDefault(u => userManager.IsInRoleAsync(u, Constantes.NombreRolAdministrador).Result) != null)
                return;

            ModeloUsuario nuevoUsuario = new ModeloUsuario
            {
	            UserName = "Administrador",
            };

            nuevoUsuario.PasswordHash = userManager.PasswordHasher.HashPassword(nuevoUsuario, @"*&//--ElJefe--\\=¿?");

            try
            {
	            await userManager.CreateAsync(nuevoUsuario);

	            await userManager.AddToRoleAsync(nuevoUsuario, Constantes.NombreRolAdministrador);

	            await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
	            dbContext.Remove(nuevoUsuario);

	            await dbContext.SaveChangesAsync();
            }
        }
    }
}
