using FluentValidation;

namespace DotForum.Application.Models.Requests.Community;

public class CreateCommunityRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}


public class CreateCommunityRequestValidator : AbstractValidator<CreateCommunityRequest>
{
    public CreateCommunityRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}