using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sales.Shared.Entidades
{
    public class Estado
    {
        public int Id { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string Nombre { get; set; } = null!;

        public Pais Pais { get; set; }

        public int PaisId { get; set; }

        public ICollection<Municipio> Municipios { get; set;}

        public int CantidadMunicipios => Municipios == null ? 0 : Municipios.Count;
    }
}
