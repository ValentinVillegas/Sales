using System.ComponentModel.DataAnnotations;

namespace Sales.Shared.Entidades
{
    public class Pais
    {
        public int Id { get; set; }

        [Display(Name = "País")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string Nombre { get; set; } = null!;
    }
}
