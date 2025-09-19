namespace SecondAPI.Domain.ViewModel;



public class LoginViewModel
{
    public string Email { get; set; }
    public string Senha { get; set; }
}

public class AuthResult
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}