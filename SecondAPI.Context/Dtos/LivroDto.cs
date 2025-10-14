namespace SecondAPI.Domain.Dtos;

public class LivroDto
{
    public string TituloLivro { get; set; } = null!;
    public List<EmprestimoDto> Emprestimos { get; set; }

}
