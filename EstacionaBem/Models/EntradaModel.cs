using System;
using System.ComponentModel.DataAnnotations;

namespace EstacionaBem.Models
{
    public class EntradaModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Informe a placa do veiculo")]
        public string placa { get; set; }
        [Required(ErrorMessage = "Informe a data e hora de chegada Formato: dd/MM/aaaa hh:mm:ss")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> chegada { get; set; }

    }
}
