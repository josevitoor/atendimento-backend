using System.Linq.Expressions;
using AtendimentoBackend.DTOs;
using AtendimentoBackend.Models;

namespace AtendimentoBackend.Repositories;

public interface IAgendamentoRepository : IRepository<Agendamento>
{
    Agendamento CreateAgendamento(Agendamento agendamento);
    PagedList<Agendamento> GetAgendamentos(PaginationParameters paginationParameters);
    IEnumerable<Agendamento> GetAllWithRelations();
    Agendamento GetWithRelations(int id);
}
