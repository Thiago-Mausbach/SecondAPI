using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SecondAPI.Domain.Mapping;
using SecondAPI.Domain.Model;
using SecondAPI.Services.Interfaces;
using SecondAPI.Services.Services;
using SecondAPI.Services.ViewServices;

namespace SecondAPI.Services
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddSecondApiServices(this IServiceCollection services)
        {
            //  -------Builder para testes locais---------- -
            //services.AddDbContext<AppDbContext>(options =>
            //options.UseInMemoryDatabase(("DefaultConnection")));

            services.AddAutoMapper(cfg => { }, typeof(Mapping).Assembly);

            services.AddCascadingAuthenticationState();
            services.AddAuthorization();
            services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login";
    });

            services.AddScoped<UsuarioViewService>();
            services.AddScoped<IPasswordHasher<DadosUsuario>, PasswordHasher<DadosUsuario>>();
            services.AddScoped<ILivroService, LivroService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            return services;
        }
    }
}
