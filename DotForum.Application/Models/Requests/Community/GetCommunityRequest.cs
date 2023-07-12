using FluentValidation;

namespace DotForum.Application.Models.Requests.Community;

public class GetCommunityRequest
{
    public string CommunityId { get; set; }
}

public class GetCommunityRequestValidator : AbstractValidator<GetCommunityRequest>
{
    public GetCommunityRequestValidator()
    {
        RuleFor(x => x.CommunityId).NotEmpty();
    }
}