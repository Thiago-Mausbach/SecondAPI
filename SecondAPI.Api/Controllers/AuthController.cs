using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecondAPI.Domain.ViewModel;
using SecondAPI.Services.Interfaces;

namespace SecondAPI.Api.Controllers;

[Route("API/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(model);
        if (!result.Sucesso)
            return Unauthorized(result.Mensagem);

        return Ok(new { token = result.Token });
    }
}
