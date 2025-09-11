using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SecondAPI.Domain.Model;
using SecondAPI.Services.Interfaces;
using SecondAPI.Services.Services;

namespace SecondAPI.Services
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddSecondApiServices(this IServiceCollection services)
        {

            //  -------Builder para testes locais---------- -
            //services.AddDbContext<AppDbContext>(options =>
            //options.UseInMemoryDatabase(("DefaultConnection")));

            services.AddScoped<IPasswordHasher<DadosUsuario>, PasswordHasher<DadosUsuario>>();
            services.AddScoped<ILivroService, LivroService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            return services;
        }
    }
}
