using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests.Home;
using DotForum.Domain.Entities;
using DotForum.Domain.Models;

namespace DotForum.Application.Services.Abstractions;

public interface IHomeService
{
    public Task<AppResponse<List<PostModel>>> GetPublicHome(GetPublicHomeRequest request);
}