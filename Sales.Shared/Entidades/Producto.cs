using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.Entidades
{
    public class Producto
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no debe exceder de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; } = null!;

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripcion")]
        [MaxLength(50, ErrorMessage = "El campo {0} no debe exceder de {1} caracteres.")]
        public string Descripcion { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Precio { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Inventario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Stock { get; set; }

        public ICollection<ProductoCategoria>? CategoriasProducto { get; set; }

        [Display(Name = "Categorías")]
        public int CantidadCategoriasProducto => CategoriasProducto == null ? 0 : CategoriasProducto.Count;

        public ICollection<ProductoImage>? ProductoImagenes { get; set; }

        [Display(Name = "Imagenes")]
        public int CantidadImagenesProducto => ProductoImagenes == null ? 0 : ProductoImagenes.Count;

        [Display(Name = "Imagen")]
        public string ImagenPrincipal => ProductoImagenes == null ? string.Empty : ProductoImagenes.FirstOrDefault()!.Image;

        public ICollection<OrdenTemporal>? OrdenesTemporales { get; set; }

        public ICollection<OrdenDetalle>? OrdenDetalles { get; set; }
    }
}
