namespace SecondAPI.Domain.Dtos
{
    public class EmprestimoDto
    {
        public int Id { get; set; }
        public string UsuarioEmail { get; set; } = null!;
        public string LivroTitulo { get; set; } = null!;
        public DateTimeOffset DataEmprestimo { get; set; }
        public DateTimeOffset DataDevolucao { get; set; }
    }
}
