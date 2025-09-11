using Microsoft.AspNetCore.Mvc;
using SecondAPI.Domain.Model;
using SecondAPI.Domain.ViewModel;
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
        if (users == null || users.Count == 0)
            return BadRequest("Nenhum usuário");
        else
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

    public async Task<ActionResult> PostAsync([FromBody] DadosUsuario user)
    {

        await _service.CriarAsync(user);
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UsuarioViewModel model)
    {
        var usuario = await _service.ValidarLoginAsync(model);

        if (usuario == null)
            return Unauthorized("Credenciais inválidas");

        return Ok(new { message = "Login válido!", usuario.Email });
    }
}