using AtendimentoBackend.Context;

namespace AtendimentoBackend.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IAtendenteRepository? _atendenteRepo;
    private IPacienteRepository? _pacienteRepo;
    private IServicoRepository? _servicoRepo;
    private IDataSemanaRepository? _dataSemanaRepo;
    private IDataServicoRepository? _dataServicoRepo;
    private IAgendamentoRepository? _agendamentoRepo;
    private IUsuarioRepository? _usuarioRepo;

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

    public IServicoRepository ServicoRepository
    {
        get
        {
            return _servicoRepo = _servicoRepo ?? new ServicoRepository(_context);
        }
    }

    public IDataSemanaRepository DataSemanaRepository
    {
        get
        {
            return _dataSemanaRepo = _dataSemanaRepo ?? new DataSemanaRepository(
                _context,
                new UnitOfWork(_context),
                new DataServicoRepository(_context),
                new ServicoRepository(_context)
            );
        }
    }

    public IDataServicoRepository DataServicoRepository
    {
        get
        {
            return _dataServicoRepo = _dataServicoRepo ?? new DataServicoRepository(_context);
        }
    }

    public IAgendamentoRepository AgendamentoRepository
    {
        get
        {
            return _agendamentoRepo = _agendamentoRepo ??
                new AgendamentoRepository(
                    _context,
                    new PacienteRepository(_context),
                    new AtendenteRepository(_context),
                    new DataServicoRepository(_context)
                );
        }
    }

    public IUsuarioRepository UsuarioRepository
    {
        get
        {
            return _usuarioRepo = _usuarioRepo ?? new UsuarioRepository(_context);
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
