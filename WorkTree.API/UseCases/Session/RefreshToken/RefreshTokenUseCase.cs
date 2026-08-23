using WorkTree.API.Contracts;
using WorkTree.Communication.Requests.Auth;
using WorkTree.Communication.Responses.Auth;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.API.UseCases.Session.RefreshToken;

public class RefreshTokenUseCase
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public RefreshTokenUseCase(ITokenService tokenService, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }


    public async Task<ResponseRefreshTokenJson> Execute(RequestRefreshTokenJson request)
    {
        Validate(request);

        var isValidTokenResult = await _tokenService.ValidateTokenAsync(request.RefreshToken);

        if (!Guid.TryParse(isValidTokenResult.subId, out var subId))
            throw new UnauthorizedErrorException("Invalid token.");


        var user = _userRepository.FindById(subId);

        if (user is null)
            throw new NotFoundErrorException("User not found.");

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken(user);

        return new ResponseRefreshTokenJson
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
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