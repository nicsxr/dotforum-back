using FluentValidation;

namespace DotForum.Application.Models.Requests.Post;

public class DeletePostRequest
{
    public string PostId { get; set; } = string.Empty;
}

public class DeletePostRequestValidator : AbstractValidator<DeletePostRequest>
{
    public DeletePostRequestValidator()
    {
        RuleFor(x => x.PostId).NotEmpty();
    }
}