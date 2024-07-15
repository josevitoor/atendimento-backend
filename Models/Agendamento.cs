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

    [ForeignKey("Servico")]
    public int ServicoId { get; set; }
    [JsonIgnore]
    public Servico? Servico { get; set; }

    [ForeignKey("DataSemana")]
    public int DataSemanaId { get; set; }
    [JsonIgnore]
    public DataSemana? DataSemana { get; set; }
}
