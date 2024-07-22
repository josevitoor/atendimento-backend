using AtendimentoBackend.Context;
using AtendimentoBackend.DTOs;
using AtendimentoBackend.Models;

namespace AtendimentoBackend.Repositories;

public class PacienteRepository : Repository<Paciente>, IPacienteRepository
{
    public PacienteRepository(AppDbContext context) : base(context)
    {        
    }

    public PagedList<Paciente> GetPacientes(PaginationParameters paginationParameters)
    {
        var pacientesOrdenadas = GetAll().OrderBy(p => p.Id).AsQueryable();
        var pacientesPaginadas = PagedList<Paciente>.ToPagedList(
            pacientesOrdenadas,
            paginationParameters.PageNumber,
            paginationParameters.PageSize
        );

        return pacientesPaginadas;
    }

    public PagedList<Paciente> getPacientesFiltroNome(PacienteFiltroDTO pacienteFiltroDto, PaginationParameters paginationParameters)
    {
        var pacientes = GetAll().AsQueryable();

        if(!string.IsNullOrEmpty(pacienteFiltroDto.Nome))
        {
            pacientes = pacientes.Where(x => x.Nome.Contains(pacienteFiltroDto.Nome));
        }

        var pacientesFiltrados = PagedList<Paciente>.ToPagedList(
            pacientes, 
            paginationParameters.PageNumber, 
            paginationParameters.PageSize
        );

        return pacientesFiltrados;
    }
}
