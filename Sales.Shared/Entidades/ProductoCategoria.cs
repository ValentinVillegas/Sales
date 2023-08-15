using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.Entidades
{
    public class ProductoCategoria
    {
        public int Id { get; set; }
        public Producto Producto { get; set; } = null!;
        public int ProductoId { get; set; }
        public Categoria Categoria { get; set; } = null!;
        public int CategoriaId { get; set; }
    }
}
