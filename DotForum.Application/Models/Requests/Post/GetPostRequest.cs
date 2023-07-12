using FluentValidation;

namespace DotForum.Application.Models.Requests.Post;

public class GetPostRequest
{
    public string Id { get; set; } = string.Empty;
}

public class GetPostRequestValidator : AbstractValidator<GetPostRequest>
{
    public GetPostRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}