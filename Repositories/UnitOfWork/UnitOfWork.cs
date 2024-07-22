using AtendimentoBackend.Context;

namespace AtendimentoBackend.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IAtendenteRepository? _atendenteRepo;
    private IPacienteRepository? _pacienteRepo;

    public AppDbContext _context;
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IAtendenteRepository AtendenteRepository
    {
        get
        {
            return _atendenteRepo = _atendenteRepo ?? new AtendenteRepository(_context);
        }
    }

    public IPacienteRepository PacienteRepository
    {
        get
        {
            return _pacienteRepo = _pacienteRepo ?? new PacienteRepository(_context);
        }
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
