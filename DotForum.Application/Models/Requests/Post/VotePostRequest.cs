using System.Diagnostics;
using DotForum.Domain.Enums;
using FluentValidation;

namespace DotForum.Application.Models.Requests.Post;

public class VotePostRequest
{
    public string PostId { get; set; }
    public VoteStatusEnum Vote { get; set; }
}

public class VotePostRequestValidator : AbstractValidator<VotePostRequest>
{
    public VotePostRequestValidator()
    {
        RuleFor(x => x.PostId).NotEmpty();
        RuleFor(x => x.Vote).NotEmpty().IsInEnum();
    }
}