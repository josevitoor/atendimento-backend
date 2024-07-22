using AtendimentoBackend.DTOs;
using AtendimentoBackend.Models;

namespace AtendimentoBackend.Repositories;

public interface IPacienteRepository : IRepository<Paciente>
{
    PagedList<Paciente> GetPacientes(PaginationParameters paginationParameters);
    PagedList<Paciente> getPacientesFiltroNome(PacienteFiltroDTO pacienteFiltroDto, PaginationParameters paginationParameters);
}
