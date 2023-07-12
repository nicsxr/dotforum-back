using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests.Comment;
using DotForum.Application.Models.Requests.Post;
using DotForum.Application.Models.Responses.Comment;
using DotForum.Domain.Entities;

namespace DotForum.Application.Services.Abstractions;

public interface ICommentService
{
    public Task<AppResponse<EmptyResponse>> CreateComment(CreateCommentRequest request);
    public Task<AppResponse<GetCommentsByPostIdResponse>> GetComment(GetCommentRequest request);
    public Task<AppResponse<List<GetCommentsByPostIdResponse>>> GetChildComments(GetChildCommentsRequest request);
    public Task<AppResponse<List<GetCommentsByPostIdResponse>>> GetCommentsByPostId(GetPostCommentsRequest request);
    public Task<AppResponse<EmptyResponse>> VoteComment(VoteCommentRequest request);
}