using System.ComponentModel.DataAnnotations;

namespace SecondAPI.Api.ViewModel;


public class UsuarioViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; } = string.Empty;
    public string? Sobrenome { get; set; }

    [Required(ErrorMessage = "O email é obrigatório")]
    public string Email { get; set; } = string.Empty;

    public string? Telefone { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Senha { get; set; } = string.Empty;
}