using Microsoft.EntityFrameworkCore;
using SecondAPI.Domain.Model;
using SecondAPI.Infra.Database.Context;
using SecondAPI.Services.Interfaces;

namespace SecondAPI.Services.Services;

public class EmprestimoService : IEmprestimoService
{
    private readonly AppDbContext _context;
    public EmprestimoService(AppDbContext context)
    {

        _context = context;
    }

    public async Task<List<LivroEmprestado>> BuscaAsync()
    {
        List<LivroEmprestado> emprestados = await _context.Emprestimos.Where(l => !l.IsDeleted).ToListAsync();
        return emprestados;

    }

    public async Task<LivroEmprestado?> BuscaIdAsync(int id)
    {
        var busca = await _context.Emprestimos.FindAsync(id);
        if (busca?.IsDeleted == true)
            return null;
        else
            return busca;
    }

    public async Task<List<LivroEmprestado>> CriarAsync(List<LivroEmprestado> emprestados)
    {
        foreach (var livro in emprestados)
        {
            livro.DataEmprestimo = DateTime.UtcNow;
            livro.DataDevolucao = DateTime.UtcNow.AddDays(7);
            livro.IsDeleted = false;
            livro.DeletedAt = null;
        }

        _context.Emprestimos.AddRange(emprestados);

        await _context.SaveChangesAsync();
        return emprestados;
    }

    public async Task<LivroEmprestado> DeletarAsync(int id, LivroEmprestado emprestado)
    {

        var busca = await _context.Emprestimos.FindAsync(id);

        if (busca == null)
        {
            return emprestado;
        }
        else
        {
            busca.IsDeleted = true;
            busca.DeletedAt = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();
            return (busca);
        }
    }
}

// ~>>>Acho que não preciso de um put
//public async Task<LivroEmprestado> AtualizarTudoAsync(int id, LivroEmprestado livro)
//{
//    var busca = await _context.Emprestimos.FindAsync(id);
//    busca!.DataDevolucao = livro.DataDevolucao;
//    busca.DataEmprestimo = livro.DataEmprestimo;
//    busca. = livro.Ano;
//    busca.Genero = livro.Genero;
//    await _context.SaveChangesAsync();
//    return livro;
//}


// >>>> Também não sei se precisa de um patch
//public async Task<DadosLivro> AtualizaParcialAsync(int id, DadosLivro livro)
//{
//    var busca = await _context.Livros.FindAsync(id);

//    if (busca == null)
//    {
//        return livro;
//    }

//    if (livro.Titulo != null && livro.Titulo != "")
//        busca.Titulo = livro.Titulo;

//    if (livro.Autor != null && livro.Autor != "")
//        busca.Autor = livro.Autor;

//    if (livro.Ano != null)
//        busca.Ano = livro.Ano;

//    if (livro.Genero != null && livro.Genero != "")
//        busca.Genero = livro.Genero;

//    await _context.SaveChangesAsync();
//    return (busca);
//}
