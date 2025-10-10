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

    public async Task<List<EmprestimoDto>> BuscaAsync()
    {
        List<EmprestimoDto> emprestimos = await _context.Emprestimos
            .Where(l => !l.IsDeleted)
            .Include(e => e.DadosUsuario)
            .Include(e => e.DadosLivro)
            .Select(e => new EmprestimoDto
            {
                Id = e.Id,
                UsuarioEmail = e.DadosUsuario.Email,
                LivroTitulo = e.DadosLivro.Titulo,
                DataEmprestimo = e.DataEmprestimo,
                DataDevolucao = e.DataDevolucao
            })
            .ToListAsync();
        return emprestimos;
    }

    public async Task<Emprestimo?> BuscaIdAsync(int id)
    {
        var busca = await _context.Emprestimos.FindAsync(id);
        if (busca?.IsDeleted == true)
            return null;
        else
            return busca;
    }

    public async Task<Emprestimo?> CriarAsync(EmprestimoDto dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.UsuarioEmail);
        var livro = await _context.Livros.FirstOrDefaultAsync(l => l.Titulo == dto.LivroTitulo);

        if (livro == null || usuario == null)
            return null;

        var emprestimo = new Emprestimo
        {
            IsDeleted = false,
            DeletedAt = null,
            DadosUsuarioId = usuario.Id,
            DadosLivro = livro,
            DataEmprestimo = DateTimeOffset.UtcNow,
            DataDevolucao = DateTimeOffset.UtcNow.AddDays(7)
        };

        _context.Emprestimos.Add(entity: emprestimo);

        await _context.SaveChangesAsync();
        return emprestimo;
    }

    public async Task<Emprestimo> DeletarAsync(int id, EmprestimoDto dto)
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


    public async Task<Emprestimo> AtualizarTudoAsync(EmprestimoDto dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.UsuarioEmail);
        var livro = await _context.Livros.FirstOrDefaultAsync(l => l.Titulo == dto.LivroTitulo);


        var busca = await _context.Emprestimos.FindAsync(dto.Id);
        busca!.DataDevolucao = dto.DataDevolucao;
        busca.DataEmprestimo = dto.DataEmprestimo;
        busca.DadosUsuarioId = usuario.Id;
        busca.DadosLivroId = livro.Id;
        await _context.SaveChangesAsync();
        return busca;
    }
}