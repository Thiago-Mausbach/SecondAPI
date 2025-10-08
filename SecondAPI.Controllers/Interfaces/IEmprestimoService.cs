using SecondAPI.Domain.Model;
using SecondAPI.Domain.Dtos;

namespace SecondAPI.Services.Interfaces;

public interface IEmprestimoService
{
    Task<List<LivroEmprestado>> BuscaAsync();
    Task<LivroEmprestado?> BuscaIdAsync(int id);
    Task<LivroEmprestado> CriarAsync(EmprestimoDto dto);
    Task<LivroEmprestado> DeletarAsync(int id, EmprestimoDto dto);
    //Task<LivroEmprestado> AtualizarTudoAsync(int id, LivroEmprestado livro);
    //Task<LivroEmprestado> AtualizaParcialAsync(int id, LivroEmprestado livro);
}