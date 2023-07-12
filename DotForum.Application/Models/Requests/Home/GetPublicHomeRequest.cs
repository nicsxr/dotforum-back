using DotForum.Application.Models.Common;
using DotForum.Domain.Models;
using FluentValidation;

namespace DotForum.Application.Models.Requests.Home;

public class GetPublicHomeRequest : PaginationRequestModel
{
    
}

public class GetPublicHomeRequestValidator : AbstractValidator<GetPublicHomeRequest>
{
    public GetPublicHomeRequestValidator()
    {
        RuleFor(x => x.Page).NotEmpty();
        RuleFor(x => x.PageSize).NotEmpty().LessThan(50);
    }
}
