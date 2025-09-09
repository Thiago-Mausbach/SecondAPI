using Microsoft.AspNetCore.Mvc;
using SecondAPI.Domain.Model;
using SecondAPI.Services.Interfaces;

namespace SecondAPI.Api.Controllers;

[Route("API/[controller]")]
[ApiController]
public class LivrosController : ControllerBase
{

    private readonly ILivroService _service;

    public LivrosController(ILivroService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DadosLivro>>> GetAsync()
    {
        var users = await _service.BuscaAsync();
        if (users == null || users.Count == 0)
            return BadRequest("Nenhum usuário");
        else
            return Ok(users);
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<DadosLivro>> GetIdAsync(int id)
    {
        var busca = await _service.BuscaIdAsync(id);
        if (busca == null)
            return NotFound($"Id {id} não encontrado.");
        else
            return Ok(busca);
    }

    [HttpPost]

    public async Task<ActionResult> PostAsync([FromBody] List<DadosLivro> biblioteca)
    {

        await _service.CriarAsync(biblioteca);
        return Ok("Livros adicionados com sucesso");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] DadosLivro livro)
    {
        if (livro == null)
            return BadRequest("Informãções do livro inválidas");

        var busca = await _service.BuscaIdAsync(id);

        if (busca == null)
            return NotFound($"{id} não encontrado.");
        else
            return Ok(await _service.AtualizarTudoAsync(id, busca));
    }

    [HttpPatch]

    public async Task<ActionResult> PatchAsync(int id, [FromBody] DadosLivro livro)
    {

        var busca = await _service.BuscaIdAsync(id);

        if (busca == null)
            return NotFound($"{id} não encontrado.");
        else
            return Ok(_service.AtualizaParcialAsync(id, livro));
    }

    [HttpDelete]

    public async Task<ActionResult> DeleteAsync(int id)
    {
        var delete = await _service.BuscaIdAsync(id);

        if (delete == null)
            return BadRequest($"{id} não econtrado");
        else
        {
            await _service.DeletarAsync(delete);
        }
        return Ok($"O livro \"{delete.Titulo}\" foi deletado");
    }
}
