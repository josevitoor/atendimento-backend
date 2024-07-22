using System.ComponentModel.DataAnnotations;

namespace AtendimentoBackend.DTOs
{
    public class ServicoRequestDTO
    {
        [Required]
        [StringLength(100)]
        public string? Nome { get; set; }
    }
}