﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.DTOs
{
    public class OrdenTemporalDTO
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public float Cantidad { get; set; } = 1;
        public string Comentarios { get; set; } = string.Empty;
    }
}
