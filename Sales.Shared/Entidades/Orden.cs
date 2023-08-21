using Newtonsoft.Json;
using Sales.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.Entidades
{
    public class Orden
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Display(Name = "Fecha/Hora")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Fecha{ get; set; }

        public Usuario? Usuario { get; set; }
        public string? UserId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Comentarios { get; set; }

        public OrdenEstatus OrdenEstatus { get; set; }
        public ICollection<OrdenDetalle>? OrdenDetalles { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Partidas")]
        public int Partidas => OrdenDetalles == null ? 0 : OrdenDetalles.Count;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        public float Cantidad => OrdenDetalles == null ? 0 : OrdenDetalles.Sum(d => d.Cantidad);

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "SubTotal")]
        public decimal SubTotal => OrdenDetalles == null ? 0 : OrdenDetalles.Sum(d => d.Importe);
    }
}