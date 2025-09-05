using SecondAPI.Domain.Model;

namespace SecondAPI.Services.Interfaces;

public interface IUsuarioService
{
    Task<List<DadosUsuario>> BuscaAsync();
    Task<DadosUsuario?> BuscaIdAsync(int id);

    Task<List<DadosUsuario>> CriarAsync(List<DadosUsuario> users);

    Task<DadosUsuario> AtualizarTudoAsync(int id, DadosUsuario user);
    Task<DadosUsuario> AtualizaParcialAsync(int id, DadosUsuario user);
    Task DeletarAsync(int id);
}
