using FluentValidation;
using WorkTree.Communication.Requests.Auth;

namespace WorkTree.API.UseCases.Session.RefreshToken;

public class RequestRefreshTokenValidator : AbstractValidator<RequestRefreshTokenJson>
{
    public RequestRefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token could not be empty.");
    }
}