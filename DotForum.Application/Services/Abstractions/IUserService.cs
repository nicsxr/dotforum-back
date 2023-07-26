using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests;
using DotForum.Application.Models.Requests.User;
using DotForum.Application.Models.Responses.User;
using DotForum.Domain.Models;

namespace DotForum.Application.Services.Abstractions;

public interface IUserService
{
    Task<AppResponse<EmptyResponse>> RegisterUser(RegisterUserRequest model);
    public Task<AppResponse<GetUserResponse>> GetUser();
    public Task<AppResponse<List<PostModel>>> GetUserPosts(GetUserPostsRequest request);

}