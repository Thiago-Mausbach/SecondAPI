using SecondAPI.Domain.Model;
using SecondAPI.Domain.ViewModel;

namespace SecondAPI.Services.Interfaces;

public interface IUsuarioService
{
    Task<List<DadosUsuario>> BuscaAsync();
    Task<DadosUsuario?> BuscaIdAsync(int id);
    Task<DadosUsuario> CriarAsync(DadosUsuario users);
    Task<DadosUsuario> AtualizarTudoAsync(int id, DadosUsuario user);
    Task<DadosUsuario> AtualizaParcialAsync(int id, DadosUsuario user);
    Task DeletarAsync(int id);
    Task<UsuarioViewModel> CriarUsuarioAsync(UsuarioViewModel usuarioVm);
}
