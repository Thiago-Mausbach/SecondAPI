using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecondAPI.Model;

namespace SecondAPI.Controllers;

[Route("API/[controller]")]
[ApiController]
public class LivrosController : ControllerBase
{
    public static List<Dados> livros = new List<Dados>();


    [HttpGet]
    public ActionResult<IEnumerable<Dados>> Get()
    {
        return Ok(livros);
    }

    [HttpGet("{id}")]

    public ActionResult<Dados> Get(int id)
    {
        var busca = livros.FirstOrDefault(x => x.Id == id);
        if (busca == null)
            return NotFound($"Id {id} não encontrado.");
        else
            return Ok(busca);
    }

    [HttpPost]

    public ActionResult Post([FromBody] List<Dados> biblioteca) 
    {
        int novoId = livros.Any() ? livros.Max(x => x.Id) + 1 : 1;
        
        foreach (var livro in biblioteca)
        {
            livro.Id = novoId++;
            livros.Add(livro);
        }

        return Created();
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Dados livro)
    {
        var busca = livros.FirstOrDefault(x => x.Id == id);
        
        if (busca == null)
        {
            return NotFound($"{id} não encontrado.");
        }
        else
        {
            busca.Titulo = livro.Titulo;
            busca.Autor = livro.Autor;
            busca.Ano = livro.Ano;
            busca.Genero = livro.Genero;
            return Ok(busca);
        }
    }

    [HttpPatch]

    public ActionResult Patch(int id, [FromBody] Dados livro)
    {

        var busca = livros.FirstOrDefault(x => x.Id == id);

        if (busca == null)
        {
            return NotFound($"{id} não encontrado.");
        }

        if (livro.Autor != null)
            busca.Autor = livro.Autor;

        if (livro.Titulo != null)
            busca.Titulo = livro.Titulo;

        if (livro.Ano != null)
            busca.Ano = livro.Ano;

        if (livro.Genero != null)
            busca.Genero = livro.Genero;

        return Ok(busca);
    }

    [HttpDelete]

    public ActionResult Delete(int id)
    {
        var busca = livros.FirstOrDefault(x => x.Id == id);
        if (busca == null)
            return NotFound();
        else
            livros.Remove(busca);
            return Ok(busca);
    }
}
