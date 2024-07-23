using AtendimentoBackend.Context;
using AtendimentoBackend.DTOs;
using AtendimentoBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AtendimentoBackend.Repositories;

public class AgendamentoRepository : Repository<Agendamento>, IAgendamentoRepository
{
    private readonly IPacienteRepository _pacienteRepo;
    private readonly IAtendenteRepository _atendenteRepo;
    private readonly IDataServicoRepository _dataServicoRepo;

    public AgendamentoRepository(
        AppDbContext context,
        IPacienteRepository pacienteRepo,
        IAtendenteRepository atendenteRepo,
        IDataServicoRepository dataServicoRepo
    ) : base(context)
    {
        _pacienteRepo = pacienteRepo;
        _atendenteRepo = atendenteRepo;
        _dataServicoRepo = dataServicoRepo;
    }

    public Agendamento CreateAgendamento(Agendamento novoAgendamento)
    {
        var paciente = _pacienteRepo.Get(x => x.Id == novoAgendamento.PacienteId);
        var atendente = _atendenteRepo.Get(x => x.Id == novoAgendamento.AtendenteId);
        var dataServico = _dataServicoRepo.Get(x => x.Id == novoAgendamento.DataServicoId);
    
        if (paciente == null || atendente == null || dataServico == null)
        {
            throw new ArgumentException("Um dos dados de relacionamento não foi encontrado.");
        }

        var agendamento = Get(
            x => x.PacienteId == paciente.Id && 
            x.AtendenteId == atendente.Id && 
            x.DataServicoId == dataServico.Id
        );

        if (agendamento != null)
        {
            throw new InvalidOperationException("Já existe um agendamento com os dados fornecidos.");
        }

        return Create(novoAgendamento);
    }

    public PagedList<Agendamento> GetAgendamentos(PaginationParameters paginationParameters)
    {
        var agendamentosOrdenadas = GetAll()
            .OrderBy(p => p.Id)
            .AsQueryable()
            .Include(o => o.Paciente); 
        var agendamentosPaginadas = PagedList<Agendamento>.ToPagedList(
            agendamentosOrdenadas,
            paginationParameters.PageNumber,
            paginationParameters.PageSize
        );

        return agendamentosPaginadas;
    }

    public PagedList<Agendamento> getAgendamentosFiltroNome(AgendamentoFiltroDTO agendamentoFiltroDto, PaginationParameters paginationParameters)
    {
        var agendamentos = GetAll().AsQueryable();

        if(!string.IsNullOrEmpty(agendamentoFiltroDto.Nome))
        {
            agendamentos = agendamentos
            .Include(x => x.Paciente)
            .Include(o => o.Atendente)
            .Include(o => o.DataServico)
                .ThenInclude(ds => ds.Servico)
            .Include(o => o.DataServico)
                .ThenInclude(ds => ds.DataSemana)
            .Where(x => 
                x.Paciente.Nome.Contains(agendamentoFiltroDto.Nome)||
                x.Atendente.Nome.Contains(agendamentoFiltroDto.Nome)||
                x.DataServico.Servico.Nome.Contains(agendamentoFiltroDto.Nome)
            );
        }

        var agendamentosFiltrados = PagedList<Agendamento>.ToPagedList(
            agendamentos,
            paginationParameters.PageNumber, 
            paginationParameters.PageSize
        );

        return agendamentosFiltrados;
    }
}
