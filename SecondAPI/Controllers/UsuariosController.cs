using Microsoft.AspNetCore.Mvc;
using SecondAPI.Context;
using SecondAPI.Model;

namespace SecondAPI.Controllers;

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
    public ActionResult<IEnumerable<DadosUsuario>> Get()
    {
        return _context.Usuarios.ToList();
    }

    [HttpGet("{id}")]

    public ActionResult<DadosUsuario> Get(int id)
    {
        var busca = _context.Usuarios.Find(id);
        if (busca == null)
            return NotFound($"Usuario de Id {id} não encontrado.");
        else
            return Ok(busca);
    }

    [HttpPost]

    public ActionResult Post([FromBody] List<DadosUsuario> users)
    {

        foreach (var user in users)
        {
            _context.Usuarios.Add(user);
        }

        _context.SaveChanges();
        return Ok("Usuários adicionados com sucesso");
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] DadosUsuario user)
    {
        if (user == null)
            return BadRequest("Informãções de usuário inválidas");

        var busca = _context.Usuarios.Find(id);

        if (busca == null)
            return NotFound($"{id} não encontrado.");

        busca.Nome = user.Nome;
        busca.Sobrenome = user.Sobrenome;
        busca.Telefone = user.Telefone;
        busca.Email = user.Email;

        _context.SaveChanges();
        return Ok(busca);
    }

    [HttpPatch]

    public ActionResult Patch(int id, [FromBody] DadosUsuario user)
    {

        var busca = _context.Usuarios.Find(id);

        if (busca == null)
        {
            return NotFound($"Usuário de Id {id} não encontrado.");
        }

        if (user.Nome!= null && user.Nome != "")
            busca.Nome = user.Nome;

        if (user.Sobrenome != null)
            busca.Sobrenome = user.Sobrenome;

        if (user.Telefone != null)
            busca.Telefone = user.Telefone;

        if (user.Email != null && user.Email != "")
            busca.Email = user.Email; //validar campo estar vindo vazio pra troca

        _context.SaveChanges();
        return Ok(busca);
    }

    [HttpDelete]

    public ActionResult Delete(int id)
    {
        var busca = _context.Usuarios.Find(id);
        if (busca == null)
            return NotFound();
        else
            _context.Usuarios.Remove(busca);

        _context.SaveChanges();
        return Ok(busca);
    }
}
