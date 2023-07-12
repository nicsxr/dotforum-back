using DotForum.Application.Helpers;
using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests.Auth;
using DotForum.Application.Models.Responses.Auth;
using DotForum.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotForum.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AppResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        return Ok(await _authService.Login(request));
    } 
    
    [HttpPost("logout")]
    [Authorize]
    public ActionResult<AppResponse<EmptyResponse>> Logout()
    {
        Response.Cookies.Delete("Auth", new CookieOptions()
        {
            Secure = true,
            SameSite = SameSiteMode.None
        });
        return Ok();
    }
    
    [HttpGet("check")]
    [Authorize]
    public IActionResult Check()
    {
        return Ok(ResponseHelper.Ok());
    }
}