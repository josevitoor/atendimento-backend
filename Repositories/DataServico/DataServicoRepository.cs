using AtendimentoBackend.Context;
using AtendimentoBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AtendimentoBackend.Repositories;

public class DataServicoRepository : Repository<DataServico>, IDataServicoRepository
{
    public DataServicoRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<DataServico> GetDataServicos(PaginationParameters paginationParameters)
    {
        var dataServicosOrdenadas = _context.Set<DataServico>()
            .AsNoTracking()
            .Include(x => x.Servico)
            .Include(x => x.DataSemana).OrderBy(p => p.Id);
        var dataServicosPaginadas = PagedList<DataServico>.ToPagedList(
            dataServicosOrdenadas,
            paginationParameters.PageNumber,
            paginationParameters.PageSize
        );

        return dataServicosPaginadas;
    }

    public PagedList<DataServico> getDataServicosFiltroId(int id, PaginationParameters paginationParameters)
    {
        var dataServicos = _context.Set<DataServico>()
            .AsNoTracking()
            .Include(x => x.Servico)
            .Include(x => x.DataSemana)
            .Where(x => x.ServicoId == id);

        var dataServicosFiltrados = PagedList<DataServico>.ToPagedList(
            dataServicos, 
            paginationParameters.PageNumber, 
            paginationParameters.PageSize
        );

        return dataServicosFiltrados;
    }

    public IEnumerable<DataServico> GetAllWithRelations()
    {
        return _context.Set<DataServico>()
            .AsNoTracking()
            .Include(x => x.Servico)
            .Include(x => x.DataSemana)
            .ToList();
    }

    public DataServico GetWithRelations(int id)
    {
        return _context.Set<DataServico>()
            .AsNoTracking()
            .Include(x => x.Servico)
            .Include(x => x.DataSemana)
            .FirstOrDefault(c => c.Id == id);
    }
}
