using System;
using System.ComponentModel.DataAnnotations;

namespace EstacionaBem.Models
{
    public class PrecoModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Informe uma data de inicio de vigência")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> vigenciaInicio { get; set; }
        [Required(ErrorMessage = "Informe uma data de fim de vigência")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> vigenciaFim { get; set; }
        [Required(ErrorMessage = "Informe um preço para a hora inicial")]
        [DataType(DataType.Currency)]
        public Nullable<float> precoHoraInicial { get; set; }
        [Required(ErrorMessage = "Informe um preço para cada hora após a primeira")]
        [DataType(DataType.Currency)]
        public Nullable<float> precoHoraAdicional { get; set; }

    }
}
