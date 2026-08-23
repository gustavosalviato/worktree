using Microsoft.AspNetCore.Identity;
using WorkTree.API.Contracts;
using WorkTree.API.Entities;
using WorkTree.Communication.Requests.Auth;
using WorkTree.Communication.Responses.Auth;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.API.UseCases.Session.Authenticate;

public class AuthenticaUseUseCase
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;


    public AuthenticaUseUseCase(ITokenService tokenService, IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public ResponseAuthenticateUserJson Execute(RequestAuthenticateUserJson request)
    {
        var user = _userRepository.FindByEmail(request.Email);

        if (user is null)
            throw new NotFoundErrorException("User not found.");


        var doesPasswordMatches = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (doesPasswordMatches == PasswordVerificationResult.Failed)
            throw new InvalidCredentialsError("Invalid credentials.");

        var token = _tokenService.Generate(user);

        var response = new ResponseAuthenticateUserJson
        {
            AccessToken = token,
        };

        return response;
    }
}