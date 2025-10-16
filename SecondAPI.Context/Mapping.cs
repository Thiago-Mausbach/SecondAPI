using SecondAPI.Domain.Dtos;
using SecondAPI.Domain.Model;

namespace SecondAPI.Domain
{
    public static class Mapping
    {
        public static LivroDto ToLivroDto(this DadosLivro value)
        {
            var result = new LivroDto
            {
                TituloLivro = value.Titulo,
                Emprestimos = value.Emprestimos.Select(e => new EmprestimoDto
                {
                    Id = e.Id,
                    LivroTitulo = e.DadosLivro.Titulo,
                    UsuarioEmail = e.DadosUsuario.Email,
                    DataEmprestimo = e.DataEmprestimo,
                    DataDevolucao = e.DataDevolucao
                }).ToList()
            };
            return result;
        }

        public static EmprestimoDto ToEmprestimoDto(this Emprestimo value)
        {
            var result = new EmprestimoDto
            {
                Id = value.Id,
                LivroTitulo = value.DadosLivro.Titulo,
                UsuarioEmail = value.DadosUsuario.Email,
                DataEmprestimo = value.DataEmprestimo,
                DataDevolucao = value.DataDevolucao
            };

            return result;
        }

    }
}
