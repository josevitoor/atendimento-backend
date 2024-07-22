namespace AtendimentoBackend.DTOs
{
    public class ServicoResponseDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public ICollection<DataServicoResponseDTO>? DataServicos { get; set; }
    }
}