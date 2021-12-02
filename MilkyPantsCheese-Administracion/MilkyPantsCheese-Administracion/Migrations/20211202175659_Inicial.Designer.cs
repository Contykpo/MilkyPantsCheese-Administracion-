﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MilkyPantsCheese;

namespace MilkyPantsCheese.Migrations
{
    [DbContext(typeof(MilkyDbContext))]
<<<<<<< HEAD:MilkyPantsCheese-Administracion/MilkyPantsCheese-Administracion/Migrations/20211202034632_Inicial.Designer.cs
    [Migration("20211202034632_Inicial")]
    partial class Inicial
=======
    [Migration("20211201224004_inicial")]
    partial class inicial
>>>>>>> Scirica:MilkyPantsCheese-Administracion/MilkyPantsCheese-Administracion/Migrations/20211201224004_inicial.Designer.cs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.12");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

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
                        .HasAnnotation("SqlServer:Identity", "(1, 1)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("IdentityUser<int>");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

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
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloCisterna", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Capacidad")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Cisterna");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloDateTimeOffsetWrapper", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Fecha")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("ModeloUsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ModeloUsuarioId");

                    b.ToTable("DateTimeOffsetWrapper");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloDatosSensorCurado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("DioxidoDeCarbono")
                        .HasPrecision(10, 5)
                        .HasColumnType("decimal(10,5)");

                    b.Property<DateTimeOffset>("Fecha")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Humedad")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)");

                    b.Property<decimal>("Temperatura")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)");

                    b.HasKey("Id");

                    b.ToTable("DatosSensorCurado");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloFermento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("EstaDisponible")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset>("FechaElaboracion")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("LoteEnElQueSeLoUtilizoId")
                        .HasColumnType("int");

                    b.Property<string>("Observaciones")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)");

                    b.Property<decimal>("Peso")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)");

                    b.Property<int?>("TipoFermentoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LoteEnElQueSeLoUtilizoId");

                    b.HasIndex("TipoFermentoId");

                    b.ToTable("Fermento");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloLoteDeLeche", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Acidez")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)");

                    b.Property<int?>("CisternaId")
                        .HasColumnType("int");

                    b.Property<bool>("EstaDisponible")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset>("Fecha")
                        .HasColumnType("datetime(6)");

                    b.Property<byte[]>("ImagenPlanilla")
<<<<<<< HEAD:MilkyPantsCheese-Administracion/MilkyPantsCheese-Administracion/Migrations/20211202034632_Inicial.Designer.cs
                        .HasMaxLength(2097062)
                        .HasColumnType("image(2097062)");
=======
                        .HasMaxLength(20971520)
                        .HasColumnType("longblob");
>>>>>>> Scirica:MilkyPantsCheese-Administracion/MilkyPantsCheese-Administracion/Migrations/20211201224004_inicial.Designer.cs

                    b.Property<string>("NotasAdicionales")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)");

                    b.Property<decimal>("PorcentajeDeAgua")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)");

                    b.Property<int?>("TamboDeProvenienciaId")
                        .HasColumnType("int");

                    b.Property<decimal>("Temperatura")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)");

                    b.HasKey("Id");

                    b.HasIndex("CisternaId");

                    b.HasIndex("TamboDeProvenienciaId");

                    b.ToTable("LoteDeLeche");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloLoteDeQueso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("FechaInicioCuracion")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("FermentoId")
                        .HasColumnType("int");

                    b.Property<int?>("LoteDeLecheId")
                        .HasColumnType("int");

                    b.Property<string>("Observaciones")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)");

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
                        .HasColumnType("int");

                    b.Property<int>("EstadoQueso")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("FechaFinCuracion")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("LoteId")
                        .HasColumnType("int");

                    b.Property<decimal>("PesoPostCurado")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)");

                    b.Property<decimal>("PesoPreCurado")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)");

                    b.HasKey("Id");

                    b.HasIndex("LoteId");

                    b.ToTable("Queso");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloRol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloTambo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("Notas")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)");

                    b.HasKey("Id");

                    b.ToTable("Tambo");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloTipoFermento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.ToTable("TipoFermento");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloTipoQueso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.ToTable("TipoQueso");
                });

            modelBuilder.Entity("MilkyPantsCheese.ModeloUsuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<long>("DuracionSesion")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("EstaHabilitado")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset>("FinSuspension")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TieneSesionAbierta")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

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
                    b.HasOne("MilkyPantsCheese.ModeloLoteDeLeche", "LoteEnElQueSeLoUtilizo")
                        .WithMany()
                        .HasForeignKey("LoteEnElQueSeLoUtilizoId");

                    b.HasOne("MilkyPantsCheese.ModeloTipoFermento", "TipoFermento")
                        .WithMany()
                        .HasForeignKey("TipoFermentoId");

                    b.Navigation("LoteEnElQueSeLoUtilizo");

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