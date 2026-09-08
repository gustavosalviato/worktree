using WorkTree.Communication.Requests.Users;
using WorkTree.Domain.Identity;
using WorkTree.Domain.Repositories;
using WorkTree.Domain.Repositories.User;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.Application.UseCases.User.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;

    public UpdateUserUseCase(IUnitOfWork unitOfWork,
        ILoggedUser loggedUser, IUserUpdateOnlyRepository updateOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _userUpdateOnlyRepository = updateOnlyRepository;
    }

    public async Task Execute(RequestUpdateUserJson request)
    {
        ValidateAndThrowOnFailures(request);

        var user = await _loggedUser.Get();

        user.Update(request.Name);

        _userUpdateOnlyRepository.UpdateProfile(user);

        await _unitOfWork.CommitAsync();
    }

    private void ValidateAndThrowOnFailures(RequestUpdateUserJson request)
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