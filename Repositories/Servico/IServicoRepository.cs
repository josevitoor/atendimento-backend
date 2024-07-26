using AtendimentoBackend.DTOs;
using AtendimentoBackend.Models;

namespace AtendimentoBackend.Repositories;

public interface IServicoRepository : IRepository<Servico>
{
    PagedList<Servico> GetServicos(PaginationParameters paginationParameters);
    PagedList<Servico> getServicosFiltroNome(ServicoFiltroDTO servicoFiltroDto, PaginationParameters paginationParameters);
    IEnumerable<Servico> GetAllWithRelations();
    Servico GetWithRelations(int id);
}
