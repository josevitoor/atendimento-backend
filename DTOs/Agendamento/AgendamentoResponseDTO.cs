using AtendimentoBackend.Models;

namespace AtendimentoBackend.DTOs;
public class AgendamentoResponseDTO
{    
    public int Id { get; set; }
    public Paciente? Paciente { get; set; }
    public Atendente? Atendente { get; set; }
    public DataServico? DataServico { get; set; }
    public DateTime Data { get; set; }
}