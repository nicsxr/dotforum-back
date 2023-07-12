using FluentValidation;

namespace DotForum.Application.Models.Requests.Auth;

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}


public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}