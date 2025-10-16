using SecondAPI.Domain.Interfaces;

namespace SecondAPI.Domain.Model;

public class DadosUsuario : ISoftDelete
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string? Sobrenome { get; set; }
    public string? Telefone { get; set; }
    public string Email { get; set; } = null!;
    public string? Senha { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public string Cargo { get; set; } = "Usuario";
    public ICollection<Emprestimo> Emprestimo { get; set; } = new List<Emprestimo>();
}