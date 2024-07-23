using System.ComponentModel.DataAnnotations;

namespace AtendimentoBackend.DTOs
{
    public class DataServicoFiltroDTO
    {
        [Required]
        public int ServicoId { get; set; }
    }
}