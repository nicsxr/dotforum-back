using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests.Post;
using DotForum.Application.Models.Responses;
using DotForum.Application.Models.Responses.Post;

namespace DotForum.Application.Services.Abstractions;

public interface IPostService
{
    Task<AppResponse<CreatePostResponse>> CreatePost(CreatePostRequest request);
    Task<AppResponse<EmptyResponse>> DeletePost(DeletePostRequest request);
    public Task<AppResponse<GetPostResponse>> GetPost(GetPostRequest request);
    public Task<AppResponse<VotePostResponse>> VotePost(VotePostRequest request);
}