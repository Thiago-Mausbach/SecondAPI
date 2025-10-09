using SecondAPI.Domain.Dtos;
using SecondAPI.Domain.Model;

namespace SecondAPI.Services.Interfaces;

public interface IEmprestimoService
{
    Task<List<EmprestimoDto>> BuscaAsync();
    Task<Emprestimo?> BuscaIdAsync(int id);
    Task<Emprestimo?> CriarAsync(EmprestimoDto dto);
    Task<Emprestimo> DeletarAsync(int id, EmprestimoDto dto);
    //Task<LivroEmprestado> AtualizarTudoAsync(int id, LivroEmprestado livro);
    //Task<LivroEmprestado> AtualizaParcialAsync(int id, LivroEmprestado livro);
}