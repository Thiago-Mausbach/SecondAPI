using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SecondAPI.Domain.Model;
using SecondAPI.Domain.ViewModel;
using SecondAPI.Infra.Database.Context;
using SecondAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecondAPI.Services.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<DadosUsuario> _passwordHasher = new();

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResult> LoginAsync(LoginViewModel model)
    {
        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Senha, model.Senha) != PasswordVerificationResult.Success)
            return new AuthResult { Sucesso = false, Mensagem = "Email ou senha incorretos" };


        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Cargo)
        }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new AuthResult { Sucesso = true, Token = tokenHandler.WriteToken(token) };
    }
}
