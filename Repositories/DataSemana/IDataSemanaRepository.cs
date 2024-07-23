using AtendimentoBackend.DTOs;
using AtendimentoBackend.Models;

namespace AtendimentoBackend.Repositories;

public interface IDataSemanaRepository : IRepository<DataSemana>
{
    ICollection<DataSemana> Create(ICollection<DataSemana> DatasSemanas, int ServicoId);
    PagedList<DataSemana> GetDataSemanas(PaginationParameters paginationParameters);
}
