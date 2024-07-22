using System.ComponentModel.DataAnnotations;

namespace AtendimentoBackend.DTOs
{
    public class DataServicoRequestDTO
    {
        [Required]
        public int DataSemanaId { get; set; }
        [Required]
        public int ServicoId { get; set; }
    }
}