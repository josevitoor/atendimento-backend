using AtendimentoBackend.Context;
using AtendimentoBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AtendimentoBackend.Repositories;

public class DataSemanaRepository : Repository<DataSemana>, IDataSemanaRepository
{
    private readonly IDataServicoRepository _dataServicoRepo;
    private readonly IServicoRepository _servicoRepo;
    private readonly IUnitOfWork _uof;
    private readonly HashSet<string> DiasDaSemana = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "segunda",
        "terça",
        "quarta",
        "quinta",
        "sexta",
        "sábado",
        "domingo"
    };

    public DataSemanaRepository(
        AppDbContext context,
        IUnitOfWork uof,
        IDataServicoRepository dataServico,
        IServicoRepository servico
    ) : base(context)
    {
        _uof = uof;
        _dataServicoRepo = dataServico;
        _servicoRepo = servico;
    }

    public PagedList<DataSemana> GetDataSemanas(PaginationParameters paginationParameters)
    {
        var dataServicosOrdenadas = GetAll()
            .OrderBy(p => p.Id)
            .AsQueryable()
            .Include(o => o.DataServicos)
                .ThenInclude(c => c.Servico);
        var dataServicosPaginadas = PagedList<DataSemana>.ToPagedList(
            dataServicosOrdenadas,
            paginationParameters.PageNumber,
            paginationParameters.PageSize
        );

        return dataServicosPaginadas;
    }

    public ICollection<DataSemana> Create(ICollection<DataSemana> DatasSemanas, int ServicoId)
    {
        var servico = _servicoRepo.Get(x => x.Id == ServicoId);
        if (servico == null)
            throw new ArgumentException("O id do serviço é inválido!");
        
        if(!DatasSemanas.All(dto => IsMatchingDay(dto.Dia)))
            throw new ArgumentException("O dia informado não é válido, use o padrão: segunda, terça, quarta...");

        var listaDataSemanasCriadas = new List<DataSemana>();
        foreach (var item in DatasSemanas)
        {
            var dataSemana = Get(
                x => x.Dia == item.Dia &&
                x.Horario == item.Horario
            );

            if (dataSemana == null)
            {
                var dataSemanaCriada = Create(item);
                _uof.Commit();
                listaDataSemanasCriadas.Add(dataSemanaCriada);
                var dataServicoNovo = new DataServico(dataSemanaCriada.Id, ServicoId);
                _dataServicoRepo.Create(dataServicoNovo);
                _uof.Commit();
            }
            else
            {
                var dataServico = _dataServicoRepo.Get(
                    x => x.DataSemanaId == dataSemana.Id &&
                    x.ServicoId == ServicoId
                );
                if (dataServico == null)
                {
                    var dataServicoNovo = new DataServico(dataSemana.Id, ServicoId);
                    _dataServicoRepo.Create(dataServicoNovo);
                    _uof.Commit();
                }
            }
        }

        return listaDataSemanasCriadas;
    }

    private bool IsMatchingDay(string? diaSemana)
    {
        if (string.IsNullOrEmpty(diaSemana))
        {
            return false;
        }

        return DiasDaSemana.Contains(diaSemana.Trim().ToLower());
    }
}
