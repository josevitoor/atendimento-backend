namespace AtendimentoBackend.Repositories;

public interface IUnitOfWork
{
    IAtendenteRepository AtendenteRepository { get; }
    IPacienteRepository PacienteRepository { get; }
    IServicoRepository ServicoRepository { get; }
    IDataSemanaRepository DataSemanaRepository { get; }
    IDataServicoRepository DataServicoRepository { get; }
    IAgendamentoRepository AgendamentoRepository { get; }
    IUsuarioRepository UsuarioRepository { get; }
    void Commit();
}
