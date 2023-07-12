using DotForum.Application.Helpers;
using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests.Auth;
using DotForum.Application.Models.Responses.Auth;
using DotForum.Application.Services.Abstractions;
using DotForum.Domain.Entities;
using DotForum.Persistence.Repositories.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DotForum.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public AuthService(IUserService userService, ITokenService tokenService, IUserRepository userRepository, SignInManager<ApplicationUser> signInManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AppResponse<LoginResponse>> Login(LoginRequest request)
    {
        var user = await _userRepository.GetUserByUsernameAsync(request.Username);
        if (user == null)
        {
            return ResponseHelper.Fail<LoginResponse>(message: "User not found.")!;
        }

        var isPasswordValid = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!isPasswordValid.Succeeded)
        {
            return ResponseHelper.Fail<LoginResponse>(message: "Invalid credentials.")!;
        }

        var token = _tokenService.GenerateToken(user);
        _httpContextAccessor.HttpContext!.Response.Cookies.Append("Auth", token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            }
        );
        
        return ResponseHelper.Ok(new LoginResponse
        {
            Token = token,
            RefreshToken = _tokenService.GenerateRefreshToken()
        });
        
        //TODO: Store the refresh token in your data store (e.g., database, cache, etc.)
    }

    public AppResponse<EmptyResponse> Logout()
    {
        _httpContextAccessor.HttpContext!.Response.Cookies.Delete("auth");
        
        return ResponseHelper.Ok();
    }
}