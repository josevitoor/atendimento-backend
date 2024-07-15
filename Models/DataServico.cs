using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AtendimentoBackend.Models;

[Table("DatasServicos")]
public class DataServico
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("DataSemana")]
    public int DataSemanaId { get; set; }
    [JsonIgnore]
    public DataSemana? DataSemana { get; set; }

    [ForeignKey("Servico")]
    public int ServicoId { get; set; }
    [JsonIgnore]
    public Servico? Servico { get; set; }
}