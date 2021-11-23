using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MilkyPantsCheese
{
	/// <summary>
	/// Dbcontext de la aplicacion
	/// </summary>
	public class MilkyDbContext : IdentityDbContext<ModeloUsuario, ModeloRol, int>
	{
		public DbSet<ModeloLoteDeLeche> LotesDeLeche { get; set; }

        public DbSet<ModeloLoteDeQueso> LotesDeQuesos { get; set; }

        public DbSet<ModeloCisterna> Cisternas { get; set; }

        public DbSet<ModeloTambo> Tambos { get; set; }

        public DbSet<ModeloFermento> Fermentos { get; set; }

        public DbSet<ModeloTipoFermento> TiposDeFermento { get; set; }

        public DbSet<ModeloDatosSensorCurado> DatosSensorCurado { get; set; }

        public MilkyDbContext(DbContextOptions<MilkyDbContext> options)
	        : base(options) {}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

            #region Usuario

            builder.Entity<IdentityUser<int>>().Property(u => u.Id).HasAnnotation("SqlServer:Identity", "(1, 1)");

            builder.Entity<ModeloUsuario>()
	            .Property(u => u.DuracionSesion)
	            .HasConversion(new TimeSpanToTicksConverter());

            builder.Entity<ModeloUsuario>()
	            .HasMany(u => u.HistorialDeIniciosDeSesion);

            builder.Entity<ModeloUsuario>()
	            .HasMany(u => u.Roles)
	            .WithMany(r => r.Usuarios);

            #endregion

            #region Lote de leche

            // LoteDeLeche
            builder.Entity<ModeloLoteDeLeche>().ToTable("LoteDeLeche");

            builder.Entity<ModeloLoteDeLeche>()
                .Property(l => l.PorcentajeDeAgua)
                .HasPrecision(3, 2);

            builder.Entity<ModeloLoteDeLeche>()
                .Property(l => l.Temperatura)
                .HasPrecision(3, 2);

            builder.Entity<ModeloLoteDeLeche>()
                .Property(l => l.Acidez)
                .HasPrecision(3, 2);

            // --- LoteDeLeche LoteDeQueso
            builder.Entity<ModeloLoteDeLeche>()
                .HasMany(p => p.LotesDeQueso)
                .WithOne(p => p.LoteDeLeche)
                .OnDelete(DeleteBehavior.Cascade);

            // --- LoteDeLeche Tambo
            builder.Entity<ModeloLoteDeLeche>()
                .HasOne(p => p.TamboDeProveniencia)
                .WithMany(p => p.LotesDeLecheDeEsteTambo)
                .OnDelete(DeleteBehavior.Cascade);

            // --- LoteDeLeche Cisterna
            builder.Entity<ModeloLoteDeLeche>()
                .HasOne(p => p.Cisterna)
                .WithMany(p => p.LotesDeLeche)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Lote de queso

            // LoteDeQueso
            builder.Entity<ModeloLoteDeQueso>().ToTable("LoteDeQueso");

            // --- LoteDeQueso LoteDeLeche
            builder.Entity<ModeloLoteDeQueso>()
                .HasOne(p => p.LoteDeLeche)
                .WithMany(p => p.LotesDeQueso)
                .OnDelete(DeleteBehavior.Cascade);

            // --- LoteDeQueso Fermento
            builder.Entity<ModeloLoteDeQueso>()
                .HasOne(p => p.Fermento);

            // --- LoteDeQueso TipoQueso
            builder.Entity<ModeloLoteDeQueso>()
                .HasOne(p => p.TipoQueso);

            // --- LoteDeQueso Queso
            builder.Entity<ModeloLoteDeQueso>()
                .HasMany(p => p.Quesos)
                .WithOne(p => p.Lote)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Queso

            // Queso
            builder.Entity<ModeloQueso>().ToTable("Queso");

            builder.Entity<ModeloQueso>()
                .Property(l => l.PesoPreCurado)
                .HasPrecision(3, 2);

            builder.Entity<ModeloQueso>()
                .Property(l => l.PesoPostCurado)
                .HasPrecision(3, 2);

            // --- Queso LoteDeQueso
            builder.Entity<ModeloQueso>()
                .HasOne(p => p.Lote)
                .WithMany(p => p.Quesos)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Tipo queso

            // TipoQueso
            builder.Entity<ModeloTipoQueso>().ToTable("TipoQueso");

            #endregion

            #region Fermento

            // Fermento
            builder.Entity<ModeloFermento>().ToTable("Fermento");

            builder.Entity<ModeloFermento>()
                .Property(l => l.Peso)
                .HasPrecision(5, 2);

            // --- Fermento TipoFermento
            builder.Entity<ModeloFermento>()
                .HasOne(p => p.TipoFermento);

            #endregion

            #region Tipo fermento

            // TipoFermento
            builder.Entity<ModeloTipoFermento>().ToTable("TipoFermento");

            #endregion

            #region Tambo

            // Tambo
            builder.Entity<ModeloTambo>().ToTable("Tambo");

            // --- Tambo LoteDeLeche
            builder.Entity<ModeloTambo>()
                .HasMany(p => p.LotesDeLecheDeEsteTambo)
                .WithOne(p => p.TamboDeProveniencia)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Cisterna

            // Cisterna:
            builder.Entity<ModeloCisterna>().ToTable("Cisterna");

            // --- Cisterna LoteDeLeche
            builder.Entity<ModeloCisterna>()
                .HasMany(p => p.LotesDeLeche)
                .WithOne(p => p.Cisterna)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region DateTimeOffset Wrapper

            builder.Entity<ModeloDateTimeOffsetWrapper>()
	            .ToTable("DateTimeOffsetWrapper");

            #endregion

            #region DatosSensorCurado

            builder.Entity<ModeloDatosSensorCurado>()
	            .ToTable("DatosSensorCurado");

            builder.Entity<ModeloDatosSensorCurado>()
	            .Property(s => s.Temperatura)
	            .HasPrecision(4, 1);

            builder.Entity<ModeloDatosSensorCurado>()
	            .Property(s => s.Humedad)
	            .HasPrecision(4, 1);

            builder.Entity<ModeloDatosSensorCurado>()
	            .Property(s => s.DioxidoDeCarbono)
	            .HasPrecision(10, 5);

            #endregion
        }
	}
}
