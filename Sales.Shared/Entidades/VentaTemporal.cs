using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.Entidades
{
    public class VentaTemporal
    {
        public int Id { get; set; }
        public Usuario? Usuario { get; set; }
        public string? IdUsuario { get; set; }
        public Producto? Producto { get; set; }
        public int IdProducto { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public float Cantidad { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Comentarios { get; set; }

        public decimal Importe => Producto == null ? 0 : Producto.Precio * (decimal)Cantidad;
    }
}
