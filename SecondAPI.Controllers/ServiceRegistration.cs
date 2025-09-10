using Microsoft.Extensions.DependencyInjection;
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

            services.AddScoped<ILivroService, LivroService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            return services;
        }
    }
}
