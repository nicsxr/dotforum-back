using DotForum.Domain.Enums;
using FluentValidation;

namespace DotForum.Application.Models.Requests.Comment;

public class VoteCommentRequest
{
    public string CommentId { get; set; }
    public VoteStatusEnum Vote { get; set; }
}

public class VoteCommentRequestValidator : AbstractValidator<VoteCommentRequest>
{
    public VoteCommentRequestValidator()
    {
        RuleFor(x => x.CommentId).NotEmpty();
        RuleFor(x => x.Vote).NotEmpty().IsInEnum();
    }
}