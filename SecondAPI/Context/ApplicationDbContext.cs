using Microsoft.EntityFrameworkCore;
using SecondAPI.Model;

namespace SecondAPI.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<DadosLivro> Livros { get; set; }
    public DbSet<DadosUsuario> Usuarios { get; set; }
}
