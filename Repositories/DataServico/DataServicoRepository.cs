using AtendimentoBackend.Context;
using AtendimentoBackend.DTOs;
using AtendimentoBackend.Models;

namespace AtendimentoBackend.Repositories;

public class DataServicoRepository : Repository<DataServico>, IDataServicoRepository
{
    public DataServicoRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<DataServico> GetDataServicos(PaginationParameters paginationParameters)
    {
        var dataServicosOrdenadas = GetAll().OrderBy(p => p.Id).AsQueryable();
        var dataServicosPaginadas = PagedList<DataServico>.ToPagedList(
            dataServicosOrdenadas,
            paginationParameters.PageNumber,
            paginationParameters.PageSize
        );

        return dataServicosPaginadas;
    }

    public PagedList<DataServico> getDataServicosFiltroId(int id, PaginationParameters paginationParameters)
    {
        var dataServicos = GetAll().AsQueryable();

        dataServicos = dataServicos.Where(x => x.ServicoId == id);

        var dataServicosFiltrados = PagedList<DataServico>.ToPagedList(
            dataServicos, 
            paginationParameters.PageNumber, 
            paginationParameters.PageSize
        );

        return dataServicosFiltrados;
    }
}
