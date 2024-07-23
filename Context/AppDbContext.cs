using AtendimentoBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AtendimentoBackend.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Paciente>? Pacientes { get; set; }
    public DbSet<Atendente>? Atendentes { get; set; }
    public DbSet<Servico>? Servicos { get; set; }
    public DbSet<DataSemana>? DatasSemana { get; set; }
    public DbSet<DataServico>? DatasServicos { get; set; }
    public DbSet<Agendamento>? Agendamentos { get; set; }
    public DbSet<Usuario>? Usuarios { get; set; }
}