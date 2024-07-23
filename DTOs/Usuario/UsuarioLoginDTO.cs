using System.ComponentModel.DataAnnotations;

namespace AtendimentoBackend.DTOs;

public class UsuarioLoginDTO
{
    [Required]
    [StringLength(80)]
    public string? Email { get; set; }

    [Required]
    public string? Senha { get; set; }
}