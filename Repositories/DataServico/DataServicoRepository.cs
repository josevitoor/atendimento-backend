using AtendimentoBackend.Context;
using AtendimentoBackend.DTOs;
using AtendimentoBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AtendimentoBackend.Repositories;

public class DataServicoRepository : Repository<DataServico>, IDataServicoRepository
{
    public DataServicoRepository(AppDbContext context) : base(context)
    {
    }
}
