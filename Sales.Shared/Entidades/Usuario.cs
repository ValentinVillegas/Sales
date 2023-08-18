using Microsoft.AspNetCore.Identity;
using Sales.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.Entidades
{
    public class Usuario:IdentityUser
    {
        [Display(Name = "Documnento")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Documento { get; set; } = null!;

        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Apellido { get; set; } = null!;

        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Direccion { get; set; } = null!;

        [Display(Name = "Foto")]
        public string? Foto { get; set; }

        [Display(Name = "Tipo de usuario")]
        public UserType TipoUsuario { get; set; }

        public Municipio? Municipio { get; set; }

        [Display(Name = "Municipio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una {0}")]
        public int MunicipioId { get; set; }

        [Display(Name = "Usuario")]
        public string NombreCompleto => $"{Nombre} {Apellido}";

        public ICollection<OrdenTemporal>? VentasTemporales { get; set; }
    }
}
