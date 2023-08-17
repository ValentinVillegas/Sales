using Sales.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.DTOs
{
    public class OrdenDTO
    {
        public int Id { get; set; }
        public OrdenEstatus OrdenEstatus { get; set; }
        public string Comentarios { get; set; } = string.Empty;
    }
}
