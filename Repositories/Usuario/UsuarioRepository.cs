using AtendimentoBackend.Context;
using AtendimentoBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AtendimentoBackend.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> GetUsuarioByLogin(string email, string senha)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
    }
}
