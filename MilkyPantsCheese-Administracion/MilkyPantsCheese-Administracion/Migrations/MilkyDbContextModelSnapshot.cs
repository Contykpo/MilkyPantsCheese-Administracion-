﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MilkyPantsCheese;

namespace MilkyPantsCheese.Migrations
{
    [DbContext(typeof(MilkyDbContext))]
    partial class MilkyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:Identity", "(1, 1)")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IdentityUser<int>");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloCisterna", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Capacidad")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Cisterna");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloDateTimeOffsetWrapper", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Fecha")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("ModeloUsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ModeloUsuarioId");

                    b.ToTable("DateTimeOffsetWrapper");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloFermento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("EstaDisponible")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("FechaElaboracion")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Observaciones")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<decimal>("Peso")
                        .HasPrecision(3, 2)
                        .HasColumnType("decimal(3,2)");

                    b.Property<int?>("TipoFermentoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TipoFermentoId");

                    b.ToTable("Fermento");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloLoteDeLeche", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Acidez")
                        .HasPrecision(5, 5)
                        .HasColumnType("decimal(5,5)");

                    b.Property<int?>("CisternaId")
                        .HasColumnType("int");

                    b.Property<bool>("EstaDisponible")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("Fecha")
                        .HasColumnType("datetimeoffset");

                    b.Property<byte[]>("ImagenPlanilla")
                        .HasMaxLength(20971520)
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("NotasAdicionales")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<decimal>("PorcentajeDeAgua")
                        .HasPrecision(5, 5)
                        .HasColumnType("decimal(5,5)");

                    b.Property<int?>("TamboDeProvenienciaId")
                        .HasColumnType("int");

                    b.Property<decimal>("Temperatura")
                        .HasPrecision(5, 5)
                        .HasColumnType("decimal(5,5)");

                    b.HasKey("Id");

                    b.HasIndex("CisternaId");

                    b.HasIndex("TamboDeProvenienciaId");

                    b.ToTable("LoteDeLeche");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloLoteDeQueso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("FechaInicioCuracion")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("FermentoId")
                        .HasColumnType("int");

                    b.Property<int?>("LoteDeLecheId")
                        .HasColumnType("int");

                    b.Property<string>("Observaciones")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<int?>("TipoQuesoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FermentoId");

                    b.HasIndex("LoteDeLecheId");

                    b.HasIndex("TipoQuesoId");

                    b.ToTable("LoteDeQueso");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloQueso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EstadoQueso")
                        .HasColumnType("int");

                    b.Property<int?>("LoteId")
                        .HasColumnType("int");

                    b.Property<decimal>("PesoPostCurado")
                        .HasPrecision(3, 2)
                        .HasColumnType("decimal(3,2)");

                    b.Property<decimal>("PesoPreCurado")
                        .HasPrecision(3, 2)
                        .HasColumnType("decimal(3,2)");

                    b.HasKey("Id");

                    b.HasIndex("LoteId");

                    b.ToTable("Queso");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloRol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloTambo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Notas")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.HasKey("Id");

                    b.ToTable("Tambo");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloTipoFermento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descripcion")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("TipoFermento");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloTipoQueso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("TipoQueso");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloUsuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("DuracionSesion")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("EstaHabilitado")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("FinSuspension")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TieneSesionAbierta")
                        .HasColumnType("bit");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ModeloRolModeloUsuario", b =>
                {
                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.Property<int>("UsuariosId")
                        .HasColumnType("int");

                    b.HasKey("RolesId", "UsuariosId");

                    b.HasIndex("UsuariosId");

                    b.ToTable("ModeloRolModeloUsuario");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("MilkyPantsCheese.ModeloRol", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("MilkyPantsCheese.ModeloUsuario", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("MilkyPantsCheese.ModeloUsuario", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("MilkyPantsCheese.ModeloRol", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MilkyPantsCheese.ModeloUsuario", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("MilkyPantsCheese.ModeloUsuario", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloDateTimeOffsetWrapper", b =>
                {
                    b.HasOne("MilkyPantsCheese.ModeloUsuario", null)
                        .WithMany("HistorialDeIniciosDeSesion")
                        .HasForeignKey("ModeloUsuarioId");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloFermento", b =>
                {
                    b.HasOne("MilkyPantsCheese.ModeloTipoFermento", "TipoFermento")
                        .WithMany()
                        .HasForeignKey("TipoFermentoId");

                    b.Navigation("TipoFermento");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloLoteDeLeche", b =>
                {
                    b.HasOne("MilkyPantsCheese.ModeloCisterna", "Cisterna")
                        .WithMany("LotesDeLeche")
                        .HasForeignKey("CisternaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MilkyPantsCheese.ModeloTambo", "TamboDeProveniencia")
                        .WithMany("LotesDeLecheDeEsteTambo")
                        .HasForeignKey("TamboDeProvenienciaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Cisterna");

                    b.Navigation("TamboDeProveniencia");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloLoteDeQueso", b =>
                {
                    b.HasOne("MilkyPantsCheese.ModeloFermento", "Fermento")
                        .WithMany()
                        .HasForeignKey("FermentoId");

                    b.HasOne("MilkyPantsCheese.ModeloLoteDeLeche", "LoteDeLeche")
                        .WithMany("LotesDeQueso")
                        .HasForeignKey("LoteDeLecheId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MilkyPantsCheese.ModeloTipoQueso", "TipoQueso")
                        .WithMany()
                        .HasForeignKey("TipoQuesoId");

                    b.Navigation("Fermento");

                    b.Navigation("LoteDeLeche");

                    b.Navigation("TipoQueso");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloQueso", b =>
                {
                    b.HasOne("MilkyPantsCheese.ModeloLoteDeQueso", "Lote")
                        .WithMany("Quesos")
                        .HasForeignKey("LoteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Lote");
                });

            modelBuilder.Entity("ModeloRolModeloUsuario", b =>
                {
                    b.HasOne("MilkyPantsCheese.ModeloRol", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MilkyPantsCheese.ModeloUsuario", null)
                        .WithMany()
                        .HasForeignKey("UsuariosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloCisterna", b =>
                {
                    b.Navigation("LotesDeLeche");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloLoteDeLeche", b =>
                {
                    b.Navigation("LotesDeQueso");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloLoteDeQueso", b =>
                {
                    b.Navigation("Quesos");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloTambo", b =>
                {
                    b.Navigation("LotesDeLecheDeEsteTambo");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloUsuario", b =>
                {
                    b.Navigation("HistorialDeIniciosDeSesion");
                });
#pragma warning restore 612, 618
        }
    }
}
