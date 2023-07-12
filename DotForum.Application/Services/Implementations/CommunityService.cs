using AutoMapper;
using DotForum.Application.Helpers;
using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests.Community;
using DotForum.Application.Models.Responses.Community;
using DotForum.Application.Services.Abstractions;
using DotForum.Domain.Entities;
using DotForum.Domain.Entities.Relationships;
using DotForum.Persistence.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;

namespace DotForum.Application.Services.Implementations;

public class CommunityService : ICommunityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly ICommunityRepository _communityRepository;
    private readonly IMapper _mapper;
    private readonly IUserCommunityRepository _userCommunityRepository;

    public CommunityService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository,
        ICommunityRepository communityRepository, IMapper mapper, IUserCommunityRepository userCommunityRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _communityRepository = communityRepository;
        _userCommunityRepository = userCommunityRepository;
        _mapper = mapper;
    }
    
    public async Task<AppResponse<CreateCommunityResponse>> CreateCommunity(CreateCommunityRequest request)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
        
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return ResponseHelper.Fail<CreateCommunityResponse>();
        
        var community = new Community
        {
            Name = request.Name,
            Description = request.Description,
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
}