using AtendimentoBackend.Context;
using AtendimentoBackend.DTOs;
using AtendimentoBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AtendimentoBackend.Repositories;

public class DataSemanaRepository : Repository<DataSemana>, IDataSemanaRepository
{
    private readonly IDataServicoRepository _dataServico;
    private readonly IServicoRepository _servico;
    private readonly IUnitOfWork _uof;

    public DataSemanaRepository(
        AppDbContext context,
        IUnitOfWork uof,
        IDataServicoRepository dataServico,
        IServicoRepository servico
    ) : base(context)
    {
        _uof = uof;
        _dataServico = dataServico;
        _servico = servico;
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
        var servico = _servico.Get(x => x.Id == ServicoId);
        if (servico == null)
            throw new ArgumentException("O id do serviço é inválido!");

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
                _dataServico.Create(dataServicoNovo);
                _uof.Commit();
            }
            else {
                var dataServico = _dataServico.Get(
                    x => x.DataSemanaId == dataSemana.Id && 
                    x.ServicoId == ServicoId
                );
                if (dataServico == null)
                {
                    var dataServicoNovo = new DataServico(dataSemana.Id, ServicoId);
                    _dataServico.Create(dataServicoNovo);
                    _uof.Commit();
                }
            }
        }

        return listaDataSemanasCriadas;
    }
}
