using Microsoft.EntityFrameworkCore;
using SecondAPI.Domain.Model;
using SecondAPI.Services.Context;
using SecondAPI.Services.Interfaces;

namespace SecondAPI.Services.Services;

public class LivroService : ILivroService
{
    private readonly AppDbContext _context;
    public LivroService(AppDbContext context)
    {

        _context = context;
    }

    public async Task<List<DadosLivro>> BuscaAsync()
    {
        return await _context.Livros.ToListAsync();
    }

    public async Task<DadosLivro?> BuscaIdAsync(int id)
    {
        var livro = await _context.Livros.FindAsync(id);
        return livro;
    }

    public async Task<List<DadosLivro>> CriarAsync(List<DadosLivro> livros)
    {

        _context.Livros.AddRange(livros);

        await _context.SaveChangesAsync();
        return livros;
    }

    public async Task<DadosLivro> AtualizarTudoAsync(int id, DadosLivro livro)
    {
        var busca = await _context.Livros.FindAsync(id);
        busca!.Titulo = livro.Titulo;
        busca.Autor = livro.Autor;
        busca.Ano = livro.Ano;
        busca.Genero = livro.Genero;
        await _context.SaveChangesAsync();
        return livro;
    }

    public async Task<DadosLivro> AtualizaParcialAsync(int id, DadosLivro livro)
    {
        var busca = await _context.Livros.FindAsync(id);

        if (busca == null)
        {
            return livro;
        }

        if (livro.Titulo != null && livro.Titulo != "")
            busca.Titulo = livro.Titulo;

        if (livro.Autor != null && livro.Autor != "")
            busca.Autor = livro.Autor;

        if (livro.Ano != null)
            busca.Ano = livro.Ano;

        if (livro.Genero != null && livro.Genero != "")
            busca.Genero = livro.Genero;

        await _context.SaveChangesAsync();
        return (busca);
    }
    public async Task<DadosLivro> DeletarAsync(DadosLivro livro)
    {

        _context.Livros.Remove(livro);

        await _context.SaveChangesAsync();
        return livro;
    }
}
