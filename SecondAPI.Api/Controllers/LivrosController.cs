using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondAPI.Context.Context;
using SecondAPI.Context.Model;

namespace SecondAPI.Controllers.Controllers;

[Route("API/[controller]")]
[ApiController]
public class LivrosController : ControllerBase
{

    private readonly AppDbContext _service;

    public LivrosController(AppDbContext service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DadosLivro>>> GetAsync()
    {
        var users = await _service.Livros.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<DadosLivro>> GetAsync(int id)
    {
        var busca = await _service.Livros.FindAsync(id);
        if (busca == null)
            return NotFound($"Id {id} não encontrado.");
        else
            return Ok(busca);
    }

    [HttpPost]

    public async Task<ActionResult> PostAsync([FromBody] List<DadosLivro> biblioteca)
    {

        foreach (var livro in biblioteca)
        {
            await _service.Livros.AddAsync(livro);
        }

        await _service.SaveChangesAsync();
        return Ok("Livros adicionados com sucesso");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] DadosLivro livro)
    {
        if (livro == null)
            return BadRequest("Informãções do livro inválidas");

        var busca = await _service.Livros.FindAsync(id);

        if (busca == null)
            return NotFound($"{id} não encontrado.");

        busca.Titulo = livro.Titulo;
        busca.Autor = livro.Autor;
        busca.Ano = livro.Ano;
        busca.Genero = livro.Genero;

        await _service.SaveChangesAsync();
        return Ok(busca);
    }

    [HttpPatch]

    public async Task<ActionResult> PatchAsync(int id, [FromBody] DadosLivro livro)
    {

        var busca = await _service.Livros.FindAsync(id);

        if (busca == null)
        {
            return NotFound($"{id} não encontrado.");
        }

        if (livro.Autor != null)
            busca.Autor = livro.Autor;

        if (livro.Titulo != null && livro.Titulo != "")
            busca.Titulo = livro.Titulo;

        if (livro.Ano != null)
            busca.Ano = livro.Ano;

        if (livro.Genero != null)
            busca.Genero = livro.Genero;

        await _service.SaveChangesAsync();
        return Ok(busca);
    }

    [HttpDelete]

    public async Task<ActionResult> DeleteAsync(int id)
    {
        var busca = await _service.Livros.FindAsync(id);
        if (busca == null)
            return NotFound();
        else
            _service.Livros.Remove(busca);

        await _service.SaveChangesAsync();
        return Ok(busca);
    }
}
