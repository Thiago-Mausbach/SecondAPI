namespace SecondAPI.Domain.Model;

public class DadosLivro
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string? Autor { get; set; }
    public int? Ano { get; set; }
    public string? Genero { get; set; }
    public int Quantidade { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public ICollection<LivroEmprestado> Emprestimo { get; set; } = new List<LivroEmprestado>();

}
