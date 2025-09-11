using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecondAPI.Domain.Model;
using SecondAPI.Domain.ViewModel;
using SecondAPI.Infra.Database.Context;
using SecondAPI.Services.Interfaces;

namespace SecondAPI.Services.Services;

public class UsuarioService : IUsuarioService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly PasswordHasher<DadosUsuario> _passwordHasher = new();
    public UsuarioService(AppDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<DadosUsuario>> BuscaAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<DadosUsuario?> BuscaIdAsync(int id)
    {
        var busca = await _context.Usuarios.FindAsync(id);
        return busca;
    }

    public async Task<DadosUsuario> CriarAsync(DadosUsuario user)
    {
        user.Senha = _passwordHasher.HashPassword(user, user.Senha!);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<DadosUsuario> AtualizarTudoAsync(int id, DadosUsuario user)
    {

        var busca = await _context.Usuarios.FindAsync(id);
        if (busca == null)
        {
            return (user);
        }

        busca.Nome = user.Nome;
        busca.Sobrenome = user.Sobrenome;
        busca.Telefone = user.Telefone;
        busca.Email = user.Email;

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<DadosUsuario> AtualizaParcialAsync(int id, DadosUsuario user)
    {
        var busca = await _context.Usuarios.FindAsync(id);

        if (busca == null)
        {
            return user;
        }

        if (user.Nome != null && user.Nome != "")
            busca.Nome = user.Nome;

        if (user.Sobrenome != null && user.Sobrenome != "")
            busca.Sobrenome = user.Sobrenome;

        if (user.Telefone != null && user.Telefone != "")
            busca.Telefone = user.Telefone;

        if (user.Email != null && user.Email != "")
            busca.Email = user.Email;

        await _context.SaveChangesAsync();
        return (busca);
    }
    public async Task DeletarAsync(int id)
    {
        var busca = await _context.Usuarios.FindAsync(id);
        if (busca == null)
            return;
        else
            _context.Usuarios.Remove(busca);

        await _context.SaveChangesAsync();
        return;
    }
    public async Task<UsuarioViewModel> CriarUsuarioAsync(UsuarioViewModel usuarioVm)
    {
        var usuario = _mapper.Map<DadosUsuario>(usuarioVm);

        usuario.Senha = _passwordHasher.HashPassword(usuario, usuario.Senha!);
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return _mapper.Map<UsuarioViewModel>(usuario);
    }
}
