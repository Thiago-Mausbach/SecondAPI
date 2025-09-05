using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondAPI.Context.Context;
using SecondAPI.Context.Model;

namespace SecondAPI.Controllers.Controllers;

[Route("API/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DadosUsuario>>> GetAsync()
    {
        var users = await _context.Usuarios.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<DadosUsuario>> GetAsync(int id)
    {
        var busca = await _context.Usuarios.FindAsync(id);
        if (busca == null)
            return NotFound($"Usuario de Id {id} não encontrado.");
        else
            return Ok(busca);
    }

    [HttpPost]

    public async Task<ActionResult> PostAsync([FromBody] List<DadosUsuario> users)
    {

        foreach (var user in users)
        {
            await _context.Usuarios.AddAsync(user);
        }

        await _context.SaveChangesAsync();
        return Ok("Usuários adicionados com sucesso");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] DadosUsuario user)
    {
        if (user == null)
            return BadRequest("Informãções de usuário inválidas");

        var busca = await _context.Usuarios.FindAsync(id);

        if (busca == null)
            return NotFound($"{id} não encontrado.");

        busca.Nome = user.Nome;
        busca.Sobrenome = user.Sobrenome;
        busca.Telefone = user.Telefone;
        busca.Email = user.Email;

        await _context.SaveChangesAsync();
        return Ok(busca);
    }

    [HttpPatch]

    public async Task<ActionResult> PatchAsync(int id, [FromBody] DadosUsuario user)
    {

        var busca = await _context.Usuarios.FindAsync(id);

        if (busca == null)
        {
            return NotFound($"Usuário de Id {id} não encontrado.");
        }

        if (user.Nome != null && user.Nome != "")
            busca.Nome = user.Nome;

        if (user.Sobrenome != null)
            busca.Sobrenome = user.Sobrenome;

        if (user.Telefone != null)
            busca.Telefone = user.Telefone;

        if (user.Email != null && user.Email != "")
            busca.Email = user.Email; //validar campo estar vindo vazio pra troca

        await _context.SaveChangesAsync();
        return Ok(busca);
    }

    [HttpDelete]

    public async Task<ActionResult> DeleteAsync(int id)
    {
        var busca = await _context.Usuarios.FindAsync(id);
        if (busca == null)
            return NotFound();
        else
            _context.Usuarios.Remove(busca);

        await _context.SaveChangesAsync();
        return Ok(busca);
    }
}
