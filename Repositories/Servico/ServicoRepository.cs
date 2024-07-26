using AtendimentoBackend.Context;
using AtendimentoBackend.DTOs;
using AtendimentoBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AtendimentoBackend.Repositories;

public class ServicoRepository : Repository<Servico>, IServicoRepository
{
    public ServicoRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<Servico> GetServicos(PaginationParameters paginationParameters)
    {
        var servicosOrdenadas = _context.Set<Servico>()
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .AsQueryable()
            .Include(o => o.DataServicos)
                .ThenInclude(c => c.DataSemana);
        var servicosPaginadas = PagedList<Servico>.ToPagedList(
            servicosOrdenadas,
            paginationParameters.PageNumber,
            paginationParameters.PageSize
        );

        return servicosPaginadas;
    }

    public PagedList<Servico> getServicosFiltroNome(ServicoFiltroDTO servicoFiltroDto, PaginationParameters paginationParameters)
    {
        var servicos = _context.Set<Servico>()
            .AsNoTracking().AsQueryable();

        if(!string.IsNullOrEmpty(servicoFiltroDto.Nome))
        {
            servicos = servicos
                .Where(x => x.Nome.Contains(servicoFiltroDto.Nome))
                .Include(o => o.DataServicos)
                    .ThenInclude(c => c.DataSemana);
        }

        var servicosFiltrados = PagedList<Servico>.ToPagedList(
            servicos, 
            paginationParameters.PageNumber, 
            paginationParameters.PageSize
        );

        return servicosFiltrados;
    }

    public IEnumerable<Servico> GetAllWithRelations()
    {
        return _context.Set<Servico>()
            .AsNoTracking()
            .Include(x => x.DataServicos)
                .ThenInclude(c => c.DataSemana)
            .ToList();
    }

    public Servico GetWithRelations(int id)
    {
        return _context.Set<Servico>()
            .AsNoTracking()
            .Include(x => x.DataServicos)
                .ThenInclude(c => c.DataSemana)
            .FirstOrDefault(c => c.Id == id);
    }
}
