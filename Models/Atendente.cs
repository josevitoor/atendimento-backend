using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtendimentoBackend.Models;

[Table("Atendentes")]
public class Atendente
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(15)]
    public string? Telefone { get; set; }
}
