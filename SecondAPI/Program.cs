using Microsoft.EntityFrameworkCore;
using SecondAPI.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// ------- Builder para testes locais -----------
//builder.Services.AddDbContext<AppDbContext>(options =>
//options.UseInMemoryDatabase(("DefaultConnection")));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()   // permite qualquer origem
            .AllowAnyMethod()   // permite GET, POST, PUT, PATCH, DELETE...
            .AllowAnyHeader();  // permite qualquer header
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
