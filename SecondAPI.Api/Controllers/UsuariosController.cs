using Microsoft.AspNetCore.Mvc;
using SecondAPI.Domain.Model;
using SecondAPI.Services.Interfaces;

namespace SecondAPI.Api.Controllers;

[Route("API/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuariosController(IUsuarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DadosUsuario>>> GetAsync()
    {
        var users = await _service.BuscaAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<DadosUsuario>> GetAsync(int id)
    {
        var busca = await _service.BuscaIdAsync(id);
        if (busca == null)
            return NotFound($"Usuario de Id {id} não encontrado.");
        else
            return Ok(busca);
    }

    [HttpPost]

    public async Task<ActionResult> PostAsync([FromBody] List<DadosUsuario> users)
    {

        await _service.CriarAsync(users);
        return Created("", "");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] DadosUsuario user)
    {
        if (user == null)
            return BadRequest("Informãções de usuário vazias; preencha todos os campos.");

        var busca = await _service.BuscaIdAsync(id);

        if (busca == null)
            return NotFound($"{id} não encontrado.");
        else
            await _service.AtualizarTudoAsync(id, user);
        return Ok(busca);
    }

    [HttpPatch]

    public async Task<ActionResult> PatchAsync(int id, [FromBody] DadosUsuario user)
    {
        var busca = await _service.BuscaIdAsync(id);

        if (busca == null)
            return BadRequest($"Usuário de id {id} não encontrado");
        else
            return Ok(await _service.AtualizaParcialAsync(id, user));

    }

    [HttpDelete]

    public async Task<ActionResult> DeleteAsync(int id)
    {
        var busca = await _service.BuscaIdAsync(id);
        if (busca == null)
            return NotFound($"Usuário de Id {id} não encontrado.");
        else
        {
            await _service.DeletarAsync(id);
            return Ok($"O usuário foi deletado");
        }
    }
}
