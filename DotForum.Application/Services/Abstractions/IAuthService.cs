using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests.Auth;
using DotForum.Application.Models.Responses.Auth;

namespace DotForum.Application.Services.Abstractions;

public interface IAuthService
{
    Task<AppResponse<LoginResponse>> Login(LoginRequest request);
    public AppResponse<EmptyResponse> Logout();
}