using System.ComponentModel;
using FluentValidation;

namespace DotForum.Domain.Models;

public class PaginationRequestModel
{
    [DefaultValue(10)]
    public int PageSize { get; set; } = 10;
    [DefaultValue(1)]
    public int Page { get; set; } = 1;
}

public class PaginationRequestModelValidator : AbstractValidator<PaginationRequestModel>
{
    public PaginationRequestModelValidator()
    {
        RuleFor(x => x.PageSize).NotEmpty().LessThan(50);
        RuleFor(x => x.Page).NotEmpty();
    }
}