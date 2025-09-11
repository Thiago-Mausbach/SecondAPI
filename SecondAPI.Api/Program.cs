using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using SecondAPI.Api.Controllers;
using SecondAPI.Domain.Mapping;
using SecondAPI.Infra.Database.Context;
using SecondAPI.Services;
namespace SecondAPI.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAutoMapper(cfg => { }, typeof(Mapping).Assembly);


        builder.Services.AddControllers()
               .ConfigureApplicationPartManager(apm =>
               {
                   apm.ApplicationParts.Add(new AssemblyPart(typeof(UsuariosController).Assembly));
                   apm.ApplicationParts.Add(new AssemblyPart(typeof(LivrosController).Assembly));
               });


        builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseCors("AllowAll");

        app.MapControllers();

        app.Run();
    }
}