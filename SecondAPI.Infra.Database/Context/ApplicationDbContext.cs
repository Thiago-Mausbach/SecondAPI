using Microsoft.EntityFrameworkCore;
using SecondAPI.Domain.Model;



namespace SecondAPI.Services.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<DadosLivro> Livros { get; set; }
    public DbSet<DadosUsuario> Usuarios { get; set; }
}