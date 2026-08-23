using WorkTree.Communication.Requests;
using WorkTree.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Identity;
using WorkTree.API.Contracts;
using WorkTree.API.Entities;
using WorkTree.Communication.Requests.Users;
using WorkTree.Communication.Responses.Users;

namespace WorkTree.API.UseCases.Users.Create;

public class CreateUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public CreateUserUseCase(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public ResponseUserJson Execute(RequestUserJson request)
    {
        Validate(request);

        var exists = _userRepository.FindByEmail(request.Email);

        if (exists is not null)
            throw new ConflictErrorException("User with this email already exists.");


        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
        };

        var passwordHashed = _passwordHasher.HashPassword(user, request.Password);

        user.PasswordHash = passwordHashed;

        _userRepository.Create(user);


        return new ResponseUserJson
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
        };
    }

    private void Validate(RequestUserJson request)
    {
        var validator = new RequestUserValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}