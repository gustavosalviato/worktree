using WorkTree.Exceptions.ExceptionsBase;
using WorkTree.API.Contracts.Repositories;
using WorkTree.Communication.Requests.Users;

namespace WorkTree.API.UseCases.Users.Update;

public class UpdateUserUseCase
{
    private readonly IUserRepository _userRepository;

    public UpdateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Execute(Guid userId, RequestUpdateUserJson request)
    {
        Validate(request);

        var user = await _userRepository.FindByIdAsync(userId);

        if (user is null)
            throw new NotFoundErrorException("User not found.");

        user.Update(request.Name);

        await _userRepository.UpdateAsync(user);
    }

    private void Validate(RequestUpdateUserJson request)
    {
        var validator = new RequestUpdateUserValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}