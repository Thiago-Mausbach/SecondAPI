using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SecondAPI.Domain.Model;



namespace SecondAPI.Infra.Database.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<DadosLivro> Livros { get; set; }
    public DbSet<DadosUsuario> Usuarios { get; set; }
    public DbSet<Emprestimo> Emprestimos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<DadosLivro>().HasQueryFilter(l => !l.IsDeleted);
        modelBuilder.Entity<DadosUsuario>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Emprestimo>().HasQueryFilter(e => !e.IsDeleted);


        modelBuilder.Entity<Emprestimo>(e =>
        {
            e.HasOne(x => x.DadosLivro)
            .WithMany(x => x.Emprestimos)
            .HasForeignKey(x => x.DadosLivroId);
        });

        base.OnModelCreating(modelBuilder);
    }
}

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=Biblioteca;User Id=sa;Password=Biblioteca@123;TrustServerCertificate=True;");

        return new AppDbContext(optionsBuilder.Options);
    }
}