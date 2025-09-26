namespace SecondAPI.Domain.Model;

public class LivroEmprestado
{
    public int Id { get; set; }
    public DadosLivro DadosLivro { get; set; } = null!;
    public int DadosLivrosId { get; set; }
    public DadosUsuario DadosUsuario { get; set; } = null!;
    public int DadosUsuarioId { get; set; }
    public DateTimeOffset DataEmprestimo { get; set; }
    public DateTimeOffset DataDevolucao { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}