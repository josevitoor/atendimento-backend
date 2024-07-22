namespace AtendimentoBackend.DTOs
{
    public class DataServicoResponseDTO
    {
        public int Id { get; set; }
        public DataSemanaResponseDTO? DataSemana { get; set; }
        public ServicoResponseDTO? Servico { get; set; }
    }
}