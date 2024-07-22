namespace AtendimentoBackend.DTOs
{
    public class DataSemanaResponseDTO
    {
        public int Id { get; set; }
        public string? Dia { get; set; }
        public string? Horario { get; set; }
        public ICollection<DataServicoResponseDTO>? DataServicos { get; set; }
    }
}