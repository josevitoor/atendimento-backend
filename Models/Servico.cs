using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AtendimentoBackend.Models;

[Table("Servicos")]
public class Servico
{
    public Servico()
    {
        DataServicos = new Collection<DataServico>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string? Nome { get; set; }

    [JsonIgnore]
    public ICollection<DataServico> DataServicos { get; set; }
}
