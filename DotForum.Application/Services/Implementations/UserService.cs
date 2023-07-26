using AutoMapper;
using DotForum.Application.Helpers;
using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests;
using DotForum.Application.Models.Requests.User;
using DotForum.Application.Models.Responses.User;
using DotForum.Application.Services.Abstractions;
using DotForum.Domain.Entities;
using DotForum.Domain.Enums;
using DotForum.Domain.Models;
using DotForum.Persistence.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotForum.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserService(UserManager<ApplicationUser> userManager, IUserRepository userRepository, 
        IHttpContextAccessor httpContextAccessor, IPostRepository postRepository, IMapper mapper)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _postRepository = postRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<AppResponse<EmptyResponse>> RegisterUser(RegisterUserRequest model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Username,
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        
        return result.Succeeded ? ResponseHelper.Ok() : ResponseHelper.Fail();
    }
    
    public async Task<AppResponse<GetUserResponse>> GetUser()
    {
        var userId = _httpContextAccessor.HttpContext!.User.GetUserId();

        var result = await _userRepository.GetByIdAsync(userId);
        if (result != null)
        {
            return ResponseHelper.Ok(new GetUserResponse
            {
                Username = result.UserName!,
                Id = result.Id
            });
        }
        return ResponseHelper.Fail<GetUserResponse>();
    }
    
    public async Task<AppResponse<List<PostModel>>> GetUserPosts(GetUserPostsRequest request)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();

        var posts = await _postRepository.GetQueryableAll()
            .Where(p => p.UserId == request.UserId)
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