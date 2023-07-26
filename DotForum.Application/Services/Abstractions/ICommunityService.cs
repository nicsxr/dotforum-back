using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests.Community;
using DotForum.Application.Models.Responses.Community;
using DotForum.Domain.Models;

namespace DotForum.Application.Services.Abstractions;

public interface ICommunityService
{
    public Task<AppResponse<CreateCommunityResponse>> CreateCommunity(CreateCommunityRequest request);
    public Task<AppResponse<GetCommunityResponse>> GetCommunity(GetCommunityRequest request);
    public Task<AppResponse<List<GetCommunityResponse>>> GetFollowingCommunities();
    public Task<AppResponse<EmptyResponse>> FollowCommunity(FollowCommunityRequest request);
    public Task<AppResponse<List<PostModel>>> GetCommunityPosts(GetCommunityPostsRequest request);

}