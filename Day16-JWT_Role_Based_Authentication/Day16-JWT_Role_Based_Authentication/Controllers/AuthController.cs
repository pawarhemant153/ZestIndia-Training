using Day16_JWT_Role_Based_Authentication.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AuthController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login(Login model)
    {
        if (model.UserName == "admin" && model.Password == "123")
        {
            var token = _tokenService.GenerateToken(model.UserName, "Admin");

            return Ok(token);
        }

        if (model.UserName == "user" && model.Password == "123")
        {
            var token = _tokenService.GenerateToken(model.UserName, "User");

            return Ok(token);
        }

        return Unauthorized();
    }
}