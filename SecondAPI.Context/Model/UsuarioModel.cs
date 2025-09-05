using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondAPI.Domain.Model;

public class UsuarioModel
{        
    public class DadosUsuario
    {
        public int Id { get; set; }

        public string Nome { get; set; } = null!;
        public string? Sobrenome { get; set; }
        public string? Telefone { get; set; }
        public string Email { get; set; } = null!;
    }
}
