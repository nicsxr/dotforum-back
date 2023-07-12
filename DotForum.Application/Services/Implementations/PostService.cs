using AutoMapper;
using DotForum.Application.Helpers;
using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests.Post;
using DotForum.Application.Models.Responses;
using DotForum.Application.Models.Responses.Post;
using DotForum.Application.Services.Abstractions;
using DotForum.Domain.Entities;
using DotForum.Domain.Entities.Relationships;
using DotForum.Domain.Enums;
using DotForum.Domain.Models;
using DotForum.Persistence.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;

namespace DotForum.Application.Services.Implementations;

public class PostService : IPostService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;
    private readonly ICommunityRepository _communityRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IUserPostReactionsRepository _userPostReactionsRepository;
    private readonly IMapper _mapper;
    
    public PostService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IUserRepository userRepository, IPostRepository postRepository,
        ICommunityRepository communityRepository, ICommentRepository commentRepository, IUserPostReactionsRepository userPostReactionsRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _postRepository = postRepository;
        _communityRepository = communityRepository;
        _commentRepository = commentRepository;
        _userPostReactionsRepository = userPostReactionsRepository;
        _mapper = mapper;
    }
    
    public async Task<AppResponse<CreatePostResponse>> CreatePost(CreatePostRequest request)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();

        if (userId == null || await _userRepository.GetByIdAsync(userId) == null)
            return ResponseHelper.Fail<CreatePostResponse>(message: "Invalid user");
        
        var community = await _communityRepository.GetByIdAsync(request.CommunityId);
        if (community == null) return ResponseHelper.Fail<CreatePostResponse>(message: "Invalid community");

        var post = new Post
        {
            Title = request.Title,
            Body = request.Body,
            UserId = userId,
            CommunityId = request.CommunityId
        };
        await _postRepository.AddAsync(post);
        return ResponseHelper.Ok(new CreatePostResponse
        {
            PostId = post.Id
        });
    }

    public async Task<AppResponse<EmptyResponse>> DeletePost(DeletePostRequest request)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();

        if (userId == null || await _userRepository.GetByIdAsync(userId) == null)
            return ResponseHelper.Fail(message: "Invalid user");
        
        var post = await _postRepository.GetByIdAsync(request.PostId);
        if (post == null) return ResponseHelper.Fail(message: "Invalid post id");

        if (post.UserId != userId)
            return ResponseHelper.Fail(message: "Invalid user");
        
        await _postRepository.DeleteAsync(post);
        return ResponseHelper.Ok();
    }
    
    public async Task<AppResponse<GetPostResponse>> GetPost(GetPostRequest request)
    {
        var post = await _postRepository.GetByIdAsync(request.Id);
        if (post == null) return ResponseHelper.Fail<GetPostResponse>(message: "Invalid post id");

        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();

        return ResponseHelper.Ok(_mapper.Map<GetPostResponse>(await _postRepository.GetPostModelById(request.Id, userId)));
    }

    public async Task<AppResponse<VotePostResponse>> VotePost(VotePostRequest request)
    {
        var userId = _httpContextAccessor.HttpContext!.User.GetUserId()!;

        var userPostReaction = await _userPostReactionsRepository.GetByIdAsync(userId, request.PostId);

        if (userPostReaction == null)
        {
            await _userPostReactionsRepository.AddAsync(new UserPostReaction
            {
                PostId = request.PostId,
                UserId = userId,
                VoteStatus = request.Vote
            });
        }else
        {
            await _userPostReactionsRepository.DeleteAsync(userPostReaction);
            if (userPostReaction.VoteStatus != request.Vote)
            {
                await _userPostReactionsRepository.AddAsync(new UserPostReaction
                {
                    PostId = request.PostId,
                    UserId = userId,
                    VoteStatus = request.Vote
                });
            }
        }
        
        return ResponseHelper.Ok(_mapper.Map<VotePostResponse>(await _postRepository.GetPostModelById(request.PostId, userId)));
    }
}