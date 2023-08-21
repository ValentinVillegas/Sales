using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.Entidades
{
    public class OrdenDetalle
    {
        public int Id { get; set; }
        public Orden Orden { get; set; }
        public int OrdenId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Comentarios { get; set; }

        public Producto Producto { get; set; }
        public int ProductoId { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Cantidad { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Importe")]
        public decimal Importe => Producto == null ? 0 : (decimal)Cantidad * Producto.Precio;
    }
}
