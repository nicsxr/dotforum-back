using DotForum.Domain.Models;
using FluentValidation;

namespace DotForum.Application.Models.Requests.User;

public class GetUserPostsRequest : PaginationRequestModel
{
    public string UserId { get; set; } = string.Empty;
}

public class GetUserPostsRequestValidator : AbstractValidator<GetUserPostsRequest>
{
    public GetUserPostsRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}