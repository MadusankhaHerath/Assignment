using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(EnhanzerApiClient enhanzer, LocationService locations, TokenService tokens) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var userLocations = await enhanzer.LoginAsync(request.Email, request.Password);
        if (userLocations is null)
            return Unauthorized(new { message = "Invalid email or password." });

        await locations.SaveAsync(userLocations);

        return new LoginResponse(tokens.CreateToken(request.Email), request.Email, userLocations);
    }
}
