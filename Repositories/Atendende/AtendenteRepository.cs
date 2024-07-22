using AtendimentoBackend.Context;
using AtendimentoBackend.DTOs;
using AtendimentoBackend.Models;

namespace AtendimentoBackend.Repositories;

public class AtendenteRepository : Repository<Atendente>, IAtendenteRepository
{
    public AtendenteRepository(AppDbContext context) : base(context)
    {        
    }

    public PagedList<Atendente> GetAtendentes(PaginationParameters paginationParameters)
    {
        var atendentesOrdenadas = GetAll().OrderBy(p => p.Id).AsQueryable();
        var atendentesPaginadas = PagedList<Atendente>.ToPagedList(
            atendentesOrdenadas,
            paginationParameters.PageNumber,
            paginationParameters.PageSize
        );

        return atendentesPaginadas;
    }

    public PagedList<Atendente> getAtendentesFiltroNome(AtendenteFiltroDTO atendenteFiltroDto, PaginationParameters paginationParameters)
    {
        var atendentes = GetAll().AsQueryable();

        if(!string.IsNullOrEmpty(atendenteFiltroDto.Nome))
        {
            atendentes = atendentes.Where(x => x.Nome.Contains(atendenteFiltroDto.Nome));
        }

        var atendentesFiltrados = PagedList<Atendente>.ToPagedList(
            atendentes, 
            paginationParameters.PageNumber, 
            paginationParameters.PageSize
        );

        return atendentesFiltrados;
    }
}
