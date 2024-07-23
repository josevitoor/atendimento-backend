using System.ComponentModel.DataAnnotations;

namespace AtendimentoBackend.DTOs;
public class AgendamentoRequestDTO
{
    [Required]
    public int PacienteId { get; set; }

    [Required]
    public int AtendenteId { get; set; }

    [Required]
    public int DataServicoId { get; set; }
}