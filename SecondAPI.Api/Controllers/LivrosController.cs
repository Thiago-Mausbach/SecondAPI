using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecondAPI.Domain;
using SecondAPI.Domain.Dtos;
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

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DadosLivro>>> GetAsync()
    {
        var livros = await _service.BuscaAsync();
        if (livros == null || livros.Count == 0)
            return BadRequest("Nenhum Livro");
        else
            return Ok(livros);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]

    public async Task<ActionResult> GetIdAsync(int id)
    {
        List<DadosLivro?>? busca = await _service.BuscaIdAsync(id);
        if (busca == null)
            return NotFound($"Id {id} não encontrado.");
        else
        {
            var resultado = busca.Select(e => new LivroDto
            {
                TituloLivro = e.Titulo,
                Emprestimos = e.Emprestimos.Select(e => new EmprestimoDto
                {
                    Id = e.Id,
                    DataEmprestimo = e.DataEmprestimo,
                    DataDevolucao = e.DataDevolucao
                }).ToList()
            }).ToList();

            resultado = busca.Select(x => x.ToDto()).ToList();

            return Ok(resultado);
        }
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]

    public async Task<ActionResult> PostAsync([FromBody] List<DadosLivro> biblioteca)
    {

        await _service.CriarAsync(biblioteca);
        return Ok("Livros adicionados com sucesso");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] DadosLivro livro)
    {
        if (livro == null)
            return BadRequest("Informãções do livro inválidas ou livro não encontrado");
        else
            return Ok(await _service.AtualizarTudoAsync(livro.Id, livro));
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch]

    public async Task<ActionResult> PatchAsync(int id, [FromBody] DadosLivro livro)
    {

        if (livro == null)
            return NotFound($"{id} não encontrado.");
        else
            return Ok(await _service.AtualizaParcialAsync(id, livro));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]

    public async Task<ActionResult> DeleteAsync(int id)
    {

        if (id == null)
            return BadRequest($"{id} não econtrado");
        else
        {
            await _service.DeletarAsync(id);
        }
        return Ok($"O livro \"{id}\" foi deletado");
    }
}