namespace SecondAPI.Domain.Dtos
{
    public class EmprestimoDto
    {
        public int Id { get; set; }
        public string UsuarioEmail { get; set; } = null!;
        public string LivroTitulo { get; set; } = null!;
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucao { get; set; }
    }
}
