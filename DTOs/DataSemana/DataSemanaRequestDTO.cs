using System.ComponentModel.DataAnnotations;

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