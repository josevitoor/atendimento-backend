using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AtendimentoBackend.Models;

[Table("Agendamentos")]
public class Agendamento
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Paciente")]
    public int PacienteId { get; set; }
    [JsonIgnore]
    public Paciente? Paciente { get; set; }

    [ForeignKey("Atendente")]
    public int AtendenteId { get; set; }
    [JsonIgnore]
    public Atendente? Atendente { get; set; }

    [ForeignKey("DataServico")]
    public int DataServicoId { get; set; }
    [JsonIgnore]
    public DataServico? DataServico { get; set; }
}
