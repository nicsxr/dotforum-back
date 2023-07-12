using FluentValidation;

namespace DotForum.Application.Models.Requests.Community;

public class FollowCommunityRequest
{
    public string CommunityId { get; set; }
}


public class FollowCommunityRequestValidator : AbstractValidator<FollowCommunityRequest>
{
    public FollowCommunityRequestValidator()
    {
        RuleFor(x => x.CommunityId).NotEmpty();
    }
}