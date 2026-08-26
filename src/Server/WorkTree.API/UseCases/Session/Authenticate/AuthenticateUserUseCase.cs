using Microsoft.AspNetCore.Identity;
using WorkTree.API.Contracts.Repositories;
using WorkTree.API.Contracts.Services;
using WorkTree.API.Entities;
using WorkTree.Communication.Requests.Auth;
using WorkTree.Communication.Responses.Auth;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.API.UseCases.Session.Authenticate;

public class AuthenticateUserUseCase
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;


    public AuthenticateUserUseCase(ITokenService tokenService, IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ResponseAuthenticateUserJson> Execute(RequestAuthenticateUserJson request)
    {
        Validate(request);

        var user = await _userRepository.FindByEmailAsync(request.Email);

        if (user is null)
            throw new InvalidCredentialsError("Invalid credentials.");


        var doesPasswordMatches = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (doesPasswordMatches == PasswordVerificationResult.Failed)
            throw new InvalidCredentialsError("Invalid credentials.");

        var result = await _tokenService.GenerateTokensAsync(user);

        var response = new ResponseAuthenticateUserJson
        {
            AccessToken = result.acessToken,
            RefreshToken = result.refreshToken,
        };

        return response;
    }


    private void Validate(RequestAuthenticateUserJson request)
    {
        var validator = new RequestAuthenticateUserValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}