using AutoMapper;
using DotForum.Application.Helpers;
using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests.Home;
using DotForum.Application.Services.Abstractions;
using DotForum.Domain.Entities;
using DotForum.Domain.Enums;
using DotForum.Domain.Models;
using DotForum.Persistence.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DotForum.Application.Services.Implementations;

public class HomeService : IHomeService
{
    private readonly IPostRepository _postRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    
    public HomeService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IPostRepository postRepository)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;

        _postRepository = postRepository;
    }
    
    public async Task<AppResponse<List<PostModel>>> GetPublicHome(GetPublicHomeRequest request)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();

        var posts = await _postRepository.GetQueryableAll()
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

// public string CommunityId { get; set; }
//     
// public string CommunityName { get; set; }
//     
// public string UserId { get; set; }
//     
// public string Username { get; set; }
//     
// public string Title { get; set; }
//     
// public string Body { get; set; }
//
// public int TotalComments { get; set; }
//     
// public int Upvotes { get; set; }
//     
// public VoteStatusEnum Vote { get; set; }
//     
// public int Downvotes { get; set; }
// public DateTime Timestamp { get; set; } = DateTime.UtcNow;