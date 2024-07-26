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
            x => 
            x.AtendenteId == atendente.Id && 
            x.DataServicoId == dataServico.Id
        );

        if (agendamento != null)
        {
            throw new ArgumentException("Já existe um agendamento marcado para esse atendente nessa data e serviço");
        }

        return Create(novoAgendamento);
    }

    public PagedList<Agendamento> GetAgendamentos(PaginationParameters paginationParameters)
    {
        var agendamentosOrdenadas = _context.Set<Agendamento>()
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .AsQueryable()
            .Include(o => o.Paciente)
            .Include(x => x.Atendente)
            .Include(o => o.DataServico)
                .ThenInclude(ds => ds.Servico)
            .Include(o => o.DataServico)
                .ThenInclude(ds => ds.DataSemana);
        var agendamentosPaginadas = PagedList<Agendamento>.ToPagedList(
            agendamentosOrdenadas,
            paginationParameters.PageNumber,
            paginationParameters.PageSize
        );

        return agendamentosPaginadas;
    }

    public IEnumerable<Agendamento> GetAllWithRelations()
    {
        return _context.Set<Agendamento>()
            .AsNoTracking()
            .Include(x => x.Paciente)
            .Include(x => x.Atendente)
            .Include(x => x.DataServico)
                .ThenInclude(ds => ds.Servico)
            .Include(x => x.DataServico)
                .ThenInclude(ds => ds.DataSemana)
            .ToList();
    }

    public Agendamento GetWithRelations(int id){
        return _context.Set<Agendamento>()
        .AsNoTracking()
        .Include(x => x.Paciente)
        .Include(x => x.Atendente)
        .Include(x => x.DataServico)
            .ThenInclude(ds => ds.Servico)
        .Include(x => x.DataServico)
            .ThenInclude(ds => ds.DataSemana)
        .FirstOrDefault(c => c.Id == id);
    }
}