using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SecondAPI.Context.Context;
using SecondAPI.Context.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondAPI.Services.Services
{
    internal class LivroServices
    {
        private readonly AppDbContext _context;
        public LivroServices(AppDbContext context) {

            _context = context;
        }

        public async Task<List<DadosLivro>> BuscaAsync()
        {
            return await _context.Livros.ToListAsync();
        }

        public async Task<DadosLivro?> BuscaIdAsync(int id)
        {
            var busca = await _context.Livros.FindAsync(id);

            return busca ?? null;
        }

        public async Task<List<DadosLivro>> CriarAsync(List<DadosLivro> livros)
        {
            
            _context.Livros.AddRange(livros);

            await _context.SaveChangesAsync();
            return livros;
        }

        public async Task<DadosLivro> AtualizarTudoAsync(int id, DadosLivro livro)
        {

            var busca = await _context.Livros.FindAsync(id);
            if (busca == null) {
                return (livro);
                    }

            busca.Titulo = livro.Titulo;
            busca.Autor = livro.Autor;
            busca.Ano = livro.Ano;
            busca.Genero = livro.Genero;

            await _context.SaveChangesAsync();
            return livro;
        }

        public async Task<DadosLivro> AtualizaParcialAsync(int id, DadosLivro livro)
        {
            var busca = await _context.Livros.FindAsync(id);

            if (busca == null)
            {
                return livro;
            }

            if (livro.Autor != null)
                busca.Autor = livro.Autor;

            if (livro.Titulo != null && livro.Titulo != "")
                busca.Titulo = livro.Titulo;

            if (livro.Ano != null)
                busca.Ano = livro.Ano;

            if (livro.Genero != null)
                busca.Genero = livro.Genero;

            await _context.SaveChangesAsync();
            return (busca);
        }
     public async Task<DadosLivro> DeletarAsync(int id)
        {
            var busca = await _context.Livros.FindAsync(id);
            if (busca == null)
                return //ve essa porra amanha;
            else
                _context.Livros.Remove(busca);

            await _context.SaveChangesAsync();
            return (busca);
        }
    }
}
