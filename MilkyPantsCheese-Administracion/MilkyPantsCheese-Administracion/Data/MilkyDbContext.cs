using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MilkyPantsCheese_Administracion.Modelos;

namespace MilkyPantsCheese
{
	/// <summary>
	/// Dbcontext de la aplicacion
	/// </summary>
	public class MilkyDbContext : IdentityDbContext<ModeloUsuario, ModeloRol, int>
	{
		public DbSet<ModeloLoteDeLeche> LotesDeLeche { get; set; }

		public MilkyDbContext(DbContextOptions<MilkyDbContext> options)
			:base(options){}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<ModeloLoteDeLeche>().ToTable("LoteDeLeche");

			builder.Entity<ModeloLoteDeLeche>()
				.Property(l => l.PorcentajeDeAgua)
				.HasPrecision(3, 2);

			builder.Entity<ModeloLoteDeLeche>()
				.Property(l => l.Temperatura)
				.HasPrecision(3, 2);
		}
	}
}
