using WorkTree.Communication.Requests.Auth;
using WorkTree.Communication.Responses.Auth;
using WorkTree.Domain.Repositories.User;
using WorkTree.Domain.Security.PasswordHashing;
using WorkTree.Exceptions;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.Application.UseCases.Authentication.Authenticate.WithEmailAndPassword;

public class AuthenticateWithEmailAndPasswordUserUseCase : IAuthenticateWithEmailAndPasswordUserUseCase
{
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    
    public AuthenticateWithEmailAndPasswordUserUseCase(IUserReadOnlyRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ResponseAuthenticateUserJson> Execute(RequestAuthenticateJson request)
    {
        var user = await _userRepository.FindByEmailAsync(request.Email);

        if (user is null)
            throw new InvalidCredentialsException(ResourceMessagesException.INVALID_CREDENTIALS);


        var doesPasswordMatches = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);

        if (!doesPasswordMatches)
            throw new InvalidCredentialsException(ResourceMessagesException.INVALID_CREDENTIALS);


        var response = new ResponseAuthenticateUserJson
        {
        };

        return response;
    }
}