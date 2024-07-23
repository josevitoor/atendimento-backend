using AtendimentoBackend.Models;

namespace AtendimentoBackend.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetUsuarioByLogin(string email, string senha);
}
