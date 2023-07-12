using DotForum.Application.Models.Requests.Home;
using DotForum.Application.Models.Requests.Post;
using DotForum.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotForum.API.Controllers;

[ApiController]
[Route("api/home")]
public class HomeController : ControllerBase
{
    private readonly IHomeService _homeService;
    
    public HomeController(IHomeService homeService)
    {
        _homeService = homeService;
    }
    
    [HttpGet("public")]
    public async Task<IActionResult> GetPublicHome([FromQuery]GetPublicHomeRequest request)
    {
        return Ok(await _homeService.GetPublicHome(request));
    }
}