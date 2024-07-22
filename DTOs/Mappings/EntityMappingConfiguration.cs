using AtendimentoBackend.Models;
using AutoMapper;

namespace AtendimentoBackend.DTOs.Mappings;
public class EntityMappingConfiguration : Profile
{
    public EntityMappingConfiguration()
    {
        CreateMap<Atendente, AtendenteRequestDTO>().ReverseMap();
        CreateMap<Atendente, AtendenteResponseDTO>().ReverseMap();

        CreateMap<Paciente, PacienteRequestDTO>().ReverseMap();
        CreateMap<Paciente, PacienteResponseDTO>().ReverseMap();

        CreateMap<DataSemana, DataSemanaRequestDTO>().ReverseMap();
        CreateMap<DataSemana, DataSemanaResponseDTO>().ReverseMap();

        CreateMap<DataServico, DataServicoRequestDTO>().ReverseMap();
        CreateMap<DataServico, DataServicoResponseDTO>().ReverseMap();

        CreateMap<Servico, ServicoRequestDTO>().ReverseMap();
        CreateMap<Servico, ServicoResponseDTO>().ReverseMap();
    }
}
