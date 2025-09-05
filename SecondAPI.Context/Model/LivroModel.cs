namespace SecondAPI.Context.Model;
using SecondAPI.Domain.Model;
using static SecondAPI.Domain.Model.UsuarioModel;

public class DadosLivro
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;

    public string? Autor { get; set; }

    public int? Ano { get; set; }

    public string? Genero { get; set; }
}

public class LivroEmprestado
{
    public DadosLivro Titulo { get; set; } = null!;

    public DateOnly DataEmprestimo { get; set; }

    public DateOnly DataDevolucao { get; set; }

    public DadosUsuario Id { get; set; } = null!;
    public DadosUsuario Nome { get; set; } = null!;
    public DadosUsuario Email { get; set; } = null!;
}
