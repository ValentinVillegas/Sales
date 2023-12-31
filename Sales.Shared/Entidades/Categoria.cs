﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.Entidades
{
    public class Categoria
    {
        public int Id { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public string Nombre { get; set; } = null!;

        public ICollection<ProductoCategoria>? ProductoCategorias { get; set; }

        [Display(Name = "Productos")]
        public int CantidadProductosCategoria => ProductoCategorias == null ? 0 : ProductoCategorias.Count;

    }
}
