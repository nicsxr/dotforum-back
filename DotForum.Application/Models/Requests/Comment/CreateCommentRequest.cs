using FluentValidation;

namespace DotForum.Application.Models.Requests.Comment;

public class CreateCommentRequest
{
    public string Text { get; set; } = string.Empty;
    public string? PostId { get; set; }
    public string? ParentCommentId { get; set; }
}

public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(x => x.Text).NotEmpty();
        RuleFor(x => x.PostId)
            .NotEmpty().When(c => c.ParentCommentId == null);
        RuleFor(x => x.ParentCommentId)
            .NotEmpty().When(c => c.PostId == null);
    }
}