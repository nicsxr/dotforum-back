using DotForum.Application.Enums;
using DotForum.Domain.Models;
using FluentValidation;

namespace DotForum.Application.Models.Requests.Home;

public class GetHomeRequest : PaginationRequestModel{
}
//
// public class GetHomeRequestValidator : AbstractValidator<GetHomeRequest>
// {
//     public GetHomeRequestValidator()
//     {
//         RuleFor(x => x.Page).NotEmpty();
//         RuleFor(x => x.PageSize).NotEmpty().LessThan(50);
//     }
// }
