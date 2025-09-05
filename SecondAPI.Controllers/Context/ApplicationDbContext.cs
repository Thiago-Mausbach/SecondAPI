using Microsoft.EntityFrameworkCore;
using SecondAPI.Context.Model;
using static SecondAPI.Domain.Model.UsuarioModel;

namespace SecondAPI.Context.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<DadosLivro> Livros { get; set; }
    public DbSet<DadosUsuario> Usuarios { get; set; }
}