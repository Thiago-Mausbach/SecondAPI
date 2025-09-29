using Microsoft.AspNetCore.Mvc;
using SecondAPI.Domain.Model;
using SecondAPI.Services.Interfaces;

namespace SecondAPI.Api.Controllers;

[Route("API/[controller]")]
[ApiController]
public class EmprestimoController : ControllerBase
{
    private readonly IEmprestimoService _service;

    public EmprestimoController(IEmprestimoService service)
    {
        _service = service;

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LivroEmprestado>>> GetAsync()
    {
        var livros = await _service.BuscaAsync();
        if (livros == null || livros.Count == 0)
            return BadRequest("Nenhum Livro");
        else
            return Ok(livros);
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<LivroEmprestado>> GetIdAsync(int id)
    {
        var busca = await _service.BuscaIdAsync(id);
        if (busca == null)
            return NotFound($"Id {id} não encontrado.");
        else
            return Ok(busca);
    }

    [HttpPost]

    public async Task<ActionResult> PostAsync([FromBody] List<LivroEmprestado> emprestados)
    {

        await _service.CriarAsync(emprestados);
        return Ok("Livros adicionados com sucesso");
    }

    //[HttpPut("{id}")]
    //public async Task<ActionResult> PutAsync(int id, [FromBody] LivroEmprestado livro)
    //{
    //    if (livro == null)
    //        return BadRequest("Informãções do livro inválidas ou livro não encontrado");
    //    else
    //        return Ok(await _service.AtualizarTudoAsync(livro.IdEmprestimo, livro));
    //}

    //[HttpPatch]

    //public async Task<ActionResult> PatchAsync(int id, [FromBody] LivroEmprestado livro)
    //{

    //    if (livro == null)
    //        return NotFound($"{id} não encontrado.");
    //    else
    //        return Ok(await _service.AtualizaParcialAsync(id, livro));
    //}

    [HttpDelete("{id}")]

    public async Task<ActionResult> DeleteAsync(int id)
    {
        var delete = await _service.BuscaIdAsync(id);

        if (delete == null)
            return BadRequest($"{id} não econtrado");
        else
        {
            await _service.DeletarAsync(delete.Id, delete);
        }
        return Ok($"O emprestimo  \"{delete.Id}\" foi deletado");
    }
}