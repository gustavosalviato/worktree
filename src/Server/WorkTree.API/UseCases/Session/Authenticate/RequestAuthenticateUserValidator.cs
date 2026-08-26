using FluentValidation;
using WorkTree.Communication.Requests.Auth;

namespace WorkTree.API.UseCases.Session.Authenticate;

public class RequestAuthenticateUserValidator : AbstractValidator<RequestAuthenticateUserJson>
{
    public RequestAuthenticateUserValidator()
    {
        RuleFor(user => user.Email).EmailAddress().WithMessage("Invalid email.");
        RuleFor(user => user.Password).NotEmpty().WithMessage("Password could not be empty.");
    }
}