using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using SecondAPI.Api.Controllers;
using SecondAPI.Infra.Database.Context;
using SecondAPI.Services;
namespace SecondAPI.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
            .Build();

        builder.Services.AddControllers()
               .ConfigureApplicationPartManager(apm =>
               {
                   apm.ApplicationParts.Add(new AssemblyPart(typeof(UsuariosController).Assembly));
                   apm.ApplicationParts.Add(new AssemblyPart(typeof(LivrosController).Assembly));
               });


        builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.WebHost.UseUrls("https://localhost:7146", "http://localhost:5000");

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        builder.Services.AddSecondApiServices();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseCors("AllowAll");

        app.MapControllers();

        app.Run();
    }
}