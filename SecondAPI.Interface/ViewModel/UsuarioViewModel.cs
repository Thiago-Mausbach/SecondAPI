using System.ComponentModel.DataAnnotations;

namespace SecondAPI.Interface.ViewModel;


public class UsuarioViewModel
{
    public string Nome { get; set; }
    public string? Sobrenome { get; set; }

    public string Email { get; set; }

    public string? Telefone { get; set; }
    public string Senha { get; set; }
}