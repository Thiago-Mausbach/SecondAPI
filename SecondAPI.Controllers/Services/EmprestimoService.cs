using Microsoft.EntityFrameworkCore;
using SecondAPI.Domain.Dtos;
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
        List<LivroEmprestado> emprestados = await _context.Emprestimos
            .Where(l => !l.IsDeleted)
            .Include(e => e.DadosUsuario)
            .Include(e => e.DadosLivro)
            .ToListAsync();
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

    public async Task<LivroEmprestado> CriarAsync(EmprestimoDto dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.UsuarioEmail);
        var livro = await _context.Livros.FirstOrDefaultAsync(l => l.Titulo == dto.LivroTitulo);

        var emprestimo = new LivroEmprestado
        {
            DadosUsuarioId = usuario.Id,
            DadosLivrosId = livro.Id,
            DataEmprestimo = DateTimeOffset.UtcNow,
            DataDevolucao = DateTimeOffset.UtcNow.AddDays(7),
            IsDeleted = false,
            DeletedAt = null
        };

        _context.Emprestimos.Add(emprestimo);

        await _context.SaveChangesAsync();
        return emprestimo;
    }

    public async Task<LivroEmprestado> DeletarAsync(int id, EmprestimoDto dto)
    {

        var busca = await _context.Emprestimos.FindAsync(id);

        if (busca == null)
        {
            return null;
        }
        else
        {
            busca.IsDeleted = true;
            busca.DeletedAt = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();
            return (busca);
        }
    }


    public async Task<LivroEmprestado> AtualizarTudoAsync(EmprestimoDto dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.UsuarioEmail);
        var livro = await _context.Livros.FirstOrDefaultAsync(l => l.Titulo == dto.LivroTitulo);


        var busca = await _context.Emprestimos.FindAsync(dto.Id);
        busca!.DataDevolucao = dto.DataDevolucao;
        busca.DataEmprestimo = dto.DataEmprestimo;
        busca.DadosUsuarioId = usuario.Id;
        busca.DadosLivrosId = livro.Id;
        await _context.SaveChangesAsync();
        return busca;
    }
}
