using AutoMapper;
using DotForum.Application.Helpers;
using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests.Community;
using DotForum.Application.Models.Responses.Community;
using DotForum.Application.Services.Abstractions;
using DotForum.Domain.Entities;
using DotForum.Domain.Entities.Relationships;
using DotForum.Domain.Enums;
using DotForum.Domain.Models;
using DotForum.Persistence.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DotForum.Application.Services.Implementations;

public class CommunityService : ICommunityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly ICommunityRepository _communityRepository;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    private readonly IUserCommunityRepository _userCommunityRepository;

    public CommunityService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository,
        ICommunityRepository communityRepository, IMapper mapper, IUserCommunityRepository userCommunityRepository, IPostRepository postRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _communityRepository = communityRepository;
        _userCommunityRepository = userCommunityRepository;
        _postRepository = postRepository;
        _mapper = mapper;
    }
    
    public async Task<AppResponse<CreateCommunityResponse>> CreateCommunity(CreateCommunityRequest request)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
        
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return ResponseHelper.Fail<CreateCommunityResponse>();

        var communityExists = await _communityRepository.GetByNormalizedName(request.Name);

        if (communityExists != null) return ResponseHelper.Fail<CreateCommunityResponse>();

        var community = new Community
        {
            Name = request.Name,
            Description = request.Description,
            NormalizedName = request.Name.ToUpper(),
            UserId = userId!
        };

        await _communityRepository.AddAsync(community);
        return ResponseHelper.Ok(new CreateCommunityResponse
        {
            CommunityId = community.Id
        });
    }

    public async Task<AppResponse<GetCommunityResponse>> GetCommunity(GetCommunityRequest request)
    {
        var community = await _communityRepository.GetByIdFullInfo(request.CommunityId);
        if (community == null) return ResponseHelper.Fail<GetCommunityResponse>()!;

        return ResponseHelper.Ok(_mapper.Map<GetCommunityResponse>(community));
    }

    public async Task<AppResponse<List<GetCommunityResponse>>> GetFollowingCommunities()
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
        
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return ResponseHelper.Fail<List<GetCommunityResponse>>();
        
        var communities = await _communityRepository.GetAllFollowedByUserAsync(user.Id);

        return ResponseHelper.Ok(_mapper.Map<List<GetCommunityResponse>>(communities));
    }

    public async Task<AppResponse<EmptyResponse>> FollowCommunity(FollowCommunityRequest request)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
        
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return ResponseHelper.Fail<EmptyResponse>();

        var userCommunities = await _userCommunityRepository.GetByIdAsync(user.Id, request.CommunityId);

        if (userCommunities != null)
        {
            return ResponseHelper.Fail(message: "You already follow this community!");
        }

        var follow = new UserCommunity()
        {
            UserId = user.Id,
            CommunityId = request.CommunityId
        };

        await _userCommunityRepository.AddAsync(follow);
        
        return ResponseHelper.Ok();
    }
    
    public async Task<AppResponse<List<PostModel>>> GetCommunityPosts(GetCommunityPostsRequest request)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();

        var posts = await _postRepository.GetQueryableAll()
            .Where(p => p.CommunityId == request.CommunityId)
            .OrderByDescending(p => p.Timestamp)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new PostModel
            {
                Id = p.Id,
                CommunityId = p.CommunityId,
                CommunityName = p.Community.Name,
                UserId = p.UserId,
                Username = p.User.UserName!,
                Title = p.Title,
                Body = p.Body,
                TotalComments = p.Comments.Count,
                Upvotes = p.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Upvote),
                Downvotes = p.Reactions.Count(r => r.VoteStatus == VoteStatusEnum.Downvote),
                Vote = p.Reactions.FirstOrDefault(r => r.UserId == userId).VoteStatus ?? VoteStatusEnum.Novote
            }).ToListAsync();
        
        return ResponseHelper.Ok(posts);
    }
}