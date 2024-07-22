using AtendimentoBackend.DTOs;
using AtendimentoBackend.Models;

namespace AtendimentoBackend.Repositories;

public interface IAtendenteRepository : IRepository<Atendente>
{
    PagedList<Atendente> GetAtendentes(PaginationParameters paginationParameters);
    PagedList<Atendente> getAtendentesFiltroNome(AtendenteFiltroDTO atendenteFiltroDto, PaginationParameters paginationParameters);
}
