namespace SecondAPI.Domain.Model;

public class DadosUsuario
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;
    public string? Sobrenome { get; set; }
    public string? Telefone { get; set; }
    public string Email { get; set; } = null!;
    public string? Senha { get; set; }
}
