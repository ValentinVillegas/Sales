using Sales.Shared.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.DTOs
{
    public class UserDTO : Usuario
    {
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe contener al menos {2} y máximo {1} carácteres.")]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no son iguales.")]
        [Display(Name = "Confirmación de contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe contener al menos {2} y máximo {1} carácteres.")]
        public string PasswordConfirm { get; set; } = null!;
    }
}
