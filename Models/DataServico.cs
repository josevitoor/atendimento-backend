using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AtendimentoBackend.Models;

[Table("DatasServicos")]
public class DataServico
{
    public DataServico(int dataSemanaId, int servicoId)
    {
        DataSemanaId = dataSemanaId;
        ServicoId = servicoId;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("DataSemana")]
    public int DataSemanaId { get; set; }
    [JsonIgnore]
    public DataSemana? DataSemana { get; set; }

    [Required]
    [ForeignKey("Servico")]
    public int ServicoId { get; set; }
    [JsonIgnore]
    public Servico? Servico { get; set; }
}