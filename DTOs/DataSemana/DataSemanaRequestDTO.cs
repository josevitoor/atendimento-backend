using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AtendimentoBackend.DTOs
{
    public class DataSemanaRequestDTO
    {
        [Required]
        [StringLength(20)]
        public string? Dia { get; set; }

        [Required]
        [StringLength(5)]
        public string? Horario { get; set; }
    }
}