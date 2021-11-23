using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MilkyPantsCheese.Pages
{
    /// <summary>
    /// Modelo de la pagina encargada de lidiar con la gestion de lotes de leche.
    /// </summary>
    public class AdministrarLotesDeLecheModel : PageModel
    {
        private readonly MilkyDbContext _dbContext;
        
        private readonly IConfiguration _config;

        /// <summary>
        /// Cisterna seleccionada para mostrar sus lotes.
        /// </summary>
        [Display(Name = "Cisterna")]
        [BindProperty]
        public int CisternaLotesId { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="userManager"></param>
        public AdministrarLotesDeLecheModel(MilkyDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config    = config;
        }

        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if(!decimal.TryParse(PorcentajeAgua, out var porcentajeAgua))
                ModelState.AddModelError(nameof(PorcentajeAgua), "Porcentaje de agua solo puede contener caracteres numericos");

            if(!decimal.TryParse(Temperatura, out var temperatura))
                ModelState.AddModelError(nameof(Temperatura), "Temperatura solo puede contener caracteres numericos");

            if(!decimal.TryParse(Acidez, out var acidez))
                ModelState.AddModelError(nameof(Acidez), "Acidez solo puede contener caracteres numericos");

            if (ImagenPlanillita is not null)
            {
                //Obtenemos el tamaño maximo permitido para un archivo desde la configuracion
                var tamañoMaximoImagen = int.Parse(_config["TamanioMaximoArchivo"]);

                //Nos aseguramos de que el tamaño del archivo subido no sea mayor al maximo
                if(ImagenPlanillita.Length > tamañoMaximoImagen)
                    ModelState.AddModelError($"{nameof(ImagenPlanillita)}", "Imagen demasiado grande");

                //Nos aseguramos de que el formato del archivo subido sea valido
                if (ImagenPlanillita.ImagenEsValida())
                    ModelState.AddModelError($"{nameof(ImagenPlanillita)}", "Imagen no valida");
            }

            if(ModelState.ErrorCount > 0)
                return Page();

            var cisternas = (from c in _dbContext.Cisternas select c).ToList();
            var tambos = (from c in _dbContext.Tambos select c).ToList();

            //Creamos el nuevo lote de leche.
            ModeloLoteDeLeche nuevoLoteDeLeche = new ModeloLoteDeLeche
            {
                Fecha               = FechaIngreso, 
                PorcentajeDeAgua    = porcentajeAgua,
                Temperatura         = temperatura,
                Acidez              = acidez,
                EstaDisponible      = EstaDisponible, 
                NotasAdicionales    = NotasAdicionales,
                Cisterna            = cisternas.Single(m => m.Id == CisternaId),
                TamboDeProveniencia = tambos.Single(m => m.Id == TamboId)
            };

            if (ImagenPlanillita is not null)
            {
                //Guardamos los bytes de la imagen
                using (var bReader = new BinaryReader(ImagenPlanillita.OpenReadStream()))
                {
                    nuevoLoteDeLeche.ImagenPlanilla = bReader.ReadBytes((int) ImagenPlanillita.Length);
                }
            }

            //Intentamos crear al lote de leche y guardarlo en la base de datos
            try
            {
                _dbContext.Attach(nuevoLoteDeLeche).State = EntityState.Added;

                cisternas.Single(m => m.Id == CisternaId).LotesDeLeche.Add(nuevoLoteDeLeche);
                
                tambos.Single(m => m.Id == TamboId).LotesDeLecheDeEsteTambo.Add(nuevoLoteDeLeche);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //Si el lote de leche ya se guardo en la base de datos, lo borramos siendo que fallo en los pasos anteriores.
                if (nuevoLoteDeLeche.Id != 0)
                    _dbContext.Remove(nuevoLoteDeLeche);

                await _dbContext.SaveChangesAsync();

                return new JsonResult("Algo salio mal");
            }

            //Recargamos la pagina.
            return RedirectToPage("AdministrarLotesDeLeche");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostFiltradoLotes()
        {
            return Partial("_Lotes", this);
        }

        #region Propiedades para la creacion / edicion de lotes de leche.

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de ingreso")]
        [BindProperty]
        public DateTimeOffset FechaIngreso { get; set; }

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Display(Name = "Temperatura (°C)")]
        [BindProperty]
        public string Temperatura { get; set; } = string.Empty;

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Display(Name = "Porcentaje de agua")]
        [BindProperty]
        public string PorcentajeAgua { get; set; } = string.Empty;

        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [Display(Name = "Acidez (°D)")]
        [BindProperty]
        public string Acidez { get; set; } = string.Empty;

        [StringLength(1024)]
        [Display(Name = "Notas adicionales")]
        [BindProperty]
        public string NotasAdicionales { get; set; } = string.Empty;

        [Display(Name = "Cisterna")]
        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [BindProperty]
        public int CisternaId { get; set; }

        [Display(Name = "Tambo")]
        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [BindProperty]
        public int TamboId { get; set; }

        [Display(Name = "Esta disponible")]
        [Required(ErrorMessage = Constantes.MensajeErrorCampoNoPuedeQuedarVacio)]
        [BindProperty]
        public bool EstaDisponible { get; set; }

        [Display(Name = "Imagen planilla")]
        [BindProperty]
        public IFormFile ImagenPlanillita { get; set; }

        #endregion
    }
}
