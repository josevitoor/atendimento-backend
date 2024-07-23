using System.ComponentModel.DataAnnotations;

namespace AtendimentoBackend.DTOs;

public class UsuarioRequestDTO
{
    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(80)]
    public string? Email { get; set; }

    [Required]
    [StringLength(20)]
    public string? Senha { get; set; }
}