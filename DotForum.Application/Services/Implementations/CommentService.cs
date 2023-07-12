using AutoMapper;
using DotForum.Application.Helpers;
using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests.Comment;
using DotForum.Application.Models.Requests.Post;
using DotForum.Application.Models.Responses.Comment;
using DotForum.Application.Services.Abstractions;
using DotForum.Domain.Entities;
using DotForum.Domain.Entities.Relationships;
using DotForum.Domain.Models;
using DotForum.Persistence.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace DotForum.Application.Services.Implementations;

public class CommentService : ICommentService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUserCommentReactionsRepository _userCommentReactionsRepository;
    private readonly IMapper _mapper;

    public CommentService(IHttpContextAccessor httpContextAccessor, ICommentRepository commentRepository, 
        IUserRepository userRepository, IPostRepository postRepository, IUserCommentReactionsRepository userCommentReactionsRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _postRepository = postRepository;
        _userCommentReactionsRepository = userCommentReactionsRepository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<AppResponse<EmptyResponse>> CreateComment(CreateCommentRequest request)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();

        if (userId == null || await _userRepository.GetByIdAsync(userId) == null)
            return ResponseHelper.Fail(message: "Invalid user");

        Post? post;
        Comment? parentComment = null;
        if (!request.PostId.IsNullOrEmpty())
        {
            post = await _postRepository.GetByIdAsync(request.PostId);
            if (post == null) return ResponseHelper.Fail(message: "Invalid PostId");
        }
        else
        {
            parentComment = await _commentRepository.GetByIdAsync(request.ParentCommentId);
            if (parentComment == null) return ResponseHelper.Fail(message: "Invalid CommentId"); 
            post = await _postRepository.GetByIdAsync(parentComment.PostId);
        }

        var comment = new Comment
        {
            ParentCommentId = parentComment?.CommentId,
            PostId = post?.Id!,
            Text = request.Text,
            UserId = userId
        };
        
        await _commentRepository.AddAsync(comment);
        return ResponseHelper.Ok();
    }

    public async Task<AppResponse<GetCommentsByPostIdResponse>> GetComment(GetCommentRequest request)
    {
        var comment = await _commentRepository.GetByIdAsync(request.CommentId);

        return ResponseHelper.Ok(_mapper.Map<GetCommentsByPostIdResponse>(comment));
    }
    
    public async Task<AppResponse<List<GetCommentsByPostIdResponse>>> GetChildComments(GetChildCommentsRequest request)
    {
        var comments = await _commentRepository.GetChildComments(request.CommentId, request.Page, request.PageSize);
        var mappedComments = await GetChildCommentsCount(_mapper.Map<List<GetCommentsByPostIdResponse>>(comments));

        return ResponseHelper.Ok(mappedComments);
    }
    
    public async Task<AppResponse<List<GetCommentsByPostIdResponse>>> GetCommentsByPostId(GetPostCommentsRequest request)
    {
        var comments = await _commentRepository.GetCommentsByPostId(request.PostId, request.Page, request.PageSize);
        var mappedComments = await GetChildCommentsCount(_mapper.Map<List<GetCommentsByPostIdResponse>>(comments));
        
        return ResponseHelper.Ok(mappedComments);
    }

    private async Task<List<GetCommentsByPostIdResponse>> GetChildCommentsCount(List<GetCommentsByPostIdResponse> comments)
    {
        foreach (var comment in comments)
        {
            foreach (var childComment in comment.ChildComments)
            {
                foreach (var grandChildComment in childComment.ChildComments)
                {
                    grandChildComment.ChildCommentsCount = await _commentRepository.GetChildCommentsCount(grandChildComment.CommentId);
                }
                childComment.ChildCommentsCount = await _commentRepository.GetChildCommentsCount(childComment.CommentId);
            }
            comment.ChildCommentsCount = await _commentRepository.GetChildCommentsCount(comment.CommentId);
        }

        return comments;
    }

    public async Task<AppResponse<EmptyResponse>> VoteComment(VoteCommentRequest request)
    {
        var userId = _httpContextAccessor.HttpContext!.User.GetUserId()!;

        var userCommentReaction = await _userCommentReactionsRepository.GetByIdAsync(userId, request.CommentId);

        if (userCommentReaction == null)
        {
            await _userCommentReactionsRepository.AddAsync(new UserCommentReaction
            {
                CommentId = request.CommentId,
                UserId = userId,
                VoteStatus = request.Vote
            });
        }else
        {
            await _userCommentReactionsRepository.DeleteAsync(userCommentReaction);
            if (userCommentReaction.VoteStatus != request.Vote)
            {
                await _userCommentReactionsRepository.AddAsync(new UserCommentReaction
                {
                    CommentId = request.CommentId,
                    UserId = userId,
                    VoteStatus = request.Vote
                });
            }
        }
        
        // return ResponseHelper.Ok(_mapper.Map<VotePostResponse>(await _postRepository.GetPostModelById(request.PostId, userId)));
        return ResponseHelper.Ok();
    }
        
}
