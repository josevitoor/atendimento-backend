using System.ComponentModel.DataAnnotations;

namespace AtendimentoBackend.DTOs;
public class AtendenteRequestDTO
{
    [Required]
    [StringLength(100)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(15)]
    public string? Telefone { get; set; }
}