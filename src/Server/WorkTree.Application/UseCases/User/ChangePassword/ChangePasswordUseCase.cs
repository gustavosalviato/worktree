using WorkTree.Communication.Requests.Users;
using WorkTree.Domain.Identity;
using WorkTree.Domain.Repositories.User;
using WorkTree.Domain.Security.PasswordHashing;
using WorkTree.Exceptions;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;

    public ChangePasswordUseCase(ILoggedUser loggedUser, IPasswordHasher passwordHasher,
        IUserUpdateOnlyRepository userUpdateOnlyRepository)
    {
        _loggedUser = loggedUser;
        _passwordHasher = passwordHasher;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var user = await _loggedUser.Get();

        ValidateAndThrowOnFailures(request, user);

        var hashedPassword = _passwordHasher.HashPassword(request.NewPassword);

        await _userUpdateOnlyRepository.UpdatePassword(user.Id, hashedPassword);
    }

    private void ValidateAndThrowOnFailures(RequestChangePasswordJson request, Domain.Entities.User user)
    {
        var validator = new ChangePasswordValidator();

        var result = validator.Validate(request);

        if (!_passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            throw new InvalidCredentialsException(ResourceMessagesException.VALIDATION_CURRENT_PASSWORD);


        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).ToList());
    }
}