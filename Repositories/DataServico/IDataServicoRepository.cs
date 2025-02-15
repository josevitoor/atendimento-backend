using AtendimentoBackend.Models;

namespace AtendimentoBackend.Repositories;

public interface IDataServicoRepository : IRepository<DataServico>
{
    PagedList<DataServico> GetDataServicos(PaginationParameters paginationParameters);
    PagedList<DataServico> getDataServicosFiltroId(int id, PaginationParameters paginationParameters);
    IEnumerable<DataServico> GetAllWithRelations();
    DataServico GetWithRelations(int id);
}
