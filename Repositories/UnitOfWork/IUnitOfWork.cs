namespace AtendimentoBackend.Repositories;

public interface IUnitOfWork
{
    IAtendenteRepository AtendenteRepository { get; }
    IPacienteRepository PacienteRepository { get; }
    void Commit();
}
