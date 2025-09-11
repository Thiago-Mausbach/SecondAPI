using SecondAPI.Domain.ViewModel;
using System.Net.Http.Json;

namespace SecondAPI.Services.ViewServices;

public class UsuarioViewService
{
    private readonly HttpClient _http;

    public UsuarioViewService(HttpClient http)
    {
        _http = http;
    }

    public async Task CriarUsuario(UsuarioViewModel usuario)
    {
        await _http.PostAsJsonAsync("API/Usuarios", usuario);
    }
}