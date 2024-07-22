using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AtendimentoBackend.Models;

[Table("DatasSemana")]
public class DataSemana
{
    public DataSemana()
    {
        DataServicos = new Collection<DataServico>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string? Dia { get; set; }

    [Required]
    [StringLength(5)]
    public string? Horario { get; set; }

    [JsonIgnore]
    public ICollection<DataServico> DataServicos { get; set; }
}