using DotForum.Domain.Models;
using FluentValidation;

namespace DotForum.Application.Models.Requests.Community;

public class GetCommunityPostsRequest : PaginationRequestModel
{
    public string CommunityId { get; set; } = string.Empty;
}

public class GetCommunityPostsRequestValidator : AbstractValidator<GetCommunityPostsRequest>
{
    public GetCommunityPostsRequestValidator()
    {
        RuleFor(x => x.CommunityId).NotEmpty();
    }
}