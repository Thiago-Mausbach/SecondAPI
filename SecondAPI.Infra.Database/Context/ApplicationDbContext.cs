using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SecondAPI.Domain.Interfaces;
using SecondAPI.Domain.Model;
using System.Linq.Expressions;



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


        var softDeleteEntities = typeof(ISoftDelete).Assembly.GetTypes()
    .Where(type => typeof(ISoftDelete)
                    .IsAssignableFrom(type)
                    && type.IsClass
                    && !type.IsAbstract);

        foreach (var softDeleteEntity in softDeleteEntities)
        {
            modelBuilder.Entity(softDeleteEntity).HasQueryFilter(
                  GenerateQueryFilterLambda(softDeleteEntity));

            modelBuilder.Entity(softDeleteEntity).HasIndex(nameof(ISoftDelete.IsDeleted));
        }

        modelBuilder.Entity<Emprestimo>(e =>
        {
            e.HasOne(x => x.DadosLivro)
            .WithMany(x => x.Emprestimos)
            .HasForeignKey(x => x.DadosLivroId);
        });

        base.OnModelCreating(modelBuilder);
    }



    private static LambdaExpression? GenerateQueryFilterLambda(Type type)
    {
        var parameter = Expression.Parameter(type, "w");
        var falseConstantValue = Expression.Constant(false);
        var propertyAccess = Expression.PropertyOrField(parameter, nameof(ISoftDelete.IsDeleted));
        var equalExpression = Expression.Equal(propertyAccess, falseConstantValue);
        var lambda = Expression.Lambda(equalExpression, parameter);

        return lambda;
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
}