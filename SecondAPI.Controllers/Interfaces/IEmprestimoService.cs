using SecondAPI.Domain.Model;

namespace SecondAPI.Services.Interfaces;

public interface IEmprestimoService
{
    Task<List<LivroEmprestado>> BuscaAsync();
    Task<LivroEmprestado?> BuscaIdAsync(int id);
    Task<List<LivroEmprestado>> CriarAsync(List<LivroEmprestado> livros);
    //Task<LivroEmprestado> AtualizarTudoAsync(int id, LivroEmprestado livro);
    //Task<LivroEmprestado> AtualizaParcialAsync(int id, LivroEmprestado livro);
    Task<LivroEmprestado> DeletarAsync(int id, LivroEmprestado livro);
}