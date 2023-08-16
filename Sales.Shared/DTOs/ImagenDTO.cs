using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.DTOs
{
    public class ImagenDTO
    {
        [Required]
        public int ProductoId { get; set; }

        [Required]
        public List<string> Imagenes { get; set; } = null!;
    }
}
