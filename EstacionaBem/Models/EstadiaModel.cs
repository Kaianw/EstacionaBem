using System;
using System.ComponentModel.DataAnnotations;

namespace EstacionaBem.Models
{
    public class EstadiaModel
    {
        public int id { get; set; }
        [Required]
        public string placa { get; set; }
        [Required]
        public Nullable<DateTime> chegada { get; set; }
        public Nullable<DateTime> saida { get; set; }
        public float preco { get; set; }
    }
}
