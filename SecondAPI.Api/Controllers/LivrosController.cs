using Microsoft.AspNetCore.Mvc;
using SecondAPI.Context;
using SecondAPI.Model;

namespace SecondAPI.Controllers;

[Route("API/[controller]")]
[ApiController]
public class LivrosController : ControllerBase
{

    private readonly AppDbContext _context;

    public LivrosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<DadosLivro>> Get()
    {
        return _context.Livros.ToList();
    }

    [HttpGet("{id}")]

    public ActionResult<DadosLivro> Get(int id)
    {
        var busca = _context.Livros.Find(id);
        if (busca == null)
            return NotFound($"Id {id} não encontrado.");
        else
            return Ok(busca);
    }

    [HttpPost]

    public ActionResult Post([FromBody] List<DadosLivro> biblioteca)
    {

        foreach (var livro in biblioteca)
        {
            _context.Livros.Add(livro);
        }

        _context.SaveChanges();
        return Ok("Livros adicionados com sucesso");
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] DadosLivro livro)
    {
        if (livro == null)
            return BadRequest("Informãções do livro inválidas");

        var busca = _context.Livros.Find(id);

        if (busca == null)
            return NotFound($"{id} não encontrado.");

        busca.Titulo = livro.Titulo;
        busca.Autor = livro.Autor;
        busca.Ano = livro.Ano;
        busca.Genero = livro.Genero;

        _context.SaveChanges();
        return Ok(busca);
    }

    [HttpPatch]

    public ActionResult Patch(int id, [FromBody] DadosLivro livro)
    {

        var busca = _context.Livros.Find(id);

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

        _context.SaveChanges();
        return Ok(busca);
    }

    [HttpDelete]

    public ActionResult Delete(int id)
    {
        var busca = _context.Livros.Find(id);
        if (busca == null)
            return NotFound();
        else
            _context.Livros.Remove(busca);

        _context.SaveChanges();
        return Ok(busca);
    }
}
