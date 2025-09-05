using SecondAPI.Domain.Model;

namespace SecondAPI.Services.Interfaces;

public interface ILivroService
{
    Task<List<DadosLivro>> BuscaAsync();
    Task<DadosLivro?> BuscaIdAsync(int id);
    Task<List<DadosLivro>> CriarAsync(List<DadosLivro> livros);
    Task<DadosLivro> AtualizarTudoAsync(int id, DadosLivro livro);
    Task<DadosLivro> AtualizaParcialAsync(int id, DadosLivro livro);
    Task<DadosLivro> DeletarAsync(DadosLivro livro);
}
