using FluentValidation;

namespace DotForum.Application.Models.Requests.Post;

public class CreatePostRequest
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string CommunityId { get; set; }
}

public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostRequestValidator()
    {
        RuleFor(x => x.Title).NotNull();
        RuleFor(x => x.Body).NotNull();
        RuleFor(x => x.CommunityId).NotEmpty();
    }
}