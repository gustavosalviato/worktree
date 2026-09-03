using WorkTree.API.UseCases.Session.RefreshToken;
using WorkTree.Communication.Requests.Auth;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.API.UseCases.Session.Logout;

public class LogoutUserUseCase
{
    // private readonly ITokenService _tokenService;

    // public LogoutUserUseCase(ITokenService tokenService) => _tokenService = tokenService;


    public async Task Execute(RequestRefreshTokenJson request)
    {
        Validate(request);

        // var success = await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);

        // if (!success)
        //     throw new UnauthorizedErrorException("Invalid token.");
    }

    private void Validate(RequestRefreshTokenJson request)
    {
        var validator = new RequestRefreshTokenValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}