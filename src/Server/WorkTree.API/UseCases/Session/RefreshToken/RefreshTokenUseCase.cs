using WorkTree.API.Contracts.Services;
using WorkTree.Communication.Requests.Auth;
using WorkTree.Communication.Responses.Auth;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.API.UseCases.Session.RefreshToken;

public class RefreshTokenUseCase
{
    private readonly ITokenService _tokenService;


    public RefreshTokenUseCase(ITokenService tokenService) => _tokenService = tokenService;


    public async Task<ResponseRefreshTokenJson> Execute(RequestRefreshTokenJson request)
    {
        Validate(request);

        var (success, newAccess, newRefresh) = await _tokenService.RefreshAsync(request.RefreshToken);

        if (!success)
            throw new UnauthorizedErrorException("Invalid token.");


        return new ResponseRefreshTokenJson
        {
            AccessToken = newAccess!,
            RefreshToken = newRefresh!,
        };
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