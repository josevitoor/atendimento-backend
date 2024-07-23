using AtendimentoBackend.Context;
using AtendimentoBackend.Models;

namespace AtendimentoBackend.Repositories;

public class DataServicoRepository : Repository<DataServico>, IDataServicoRepository
{
    public DataServicoRepository(AppDbContext context) : base(context)
    {
    }
}
