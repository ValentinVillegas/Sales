using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.Entidades
{
    public class ProductoImage
    {
        public int Id { get; set; }
        public Producto Producto { get; set; } = null!;
        public int ProductoId { get; set; }
        [Display(Name = "Imagen")]
        public string Image { get; set; } = null!;
    }
}
