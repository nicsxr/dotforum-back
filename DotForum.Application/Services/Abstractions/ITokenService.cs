using DotForum.Domain.Entities;

namespace DotForum.Application.Services.Abstractions;

public interface ITokenService
{
    public string GenerateToken(ApplicationUser user);
    public string GenerateRefreshToken();
}