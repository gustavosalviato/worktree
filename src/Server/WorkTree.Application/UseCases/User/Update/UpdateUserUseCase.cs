using WorkTree.Communication.Requests.Users;
using WorkTree.Domain.Repositories;
using WorkTree.Domain.Repositories.User;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.Application.UseCases.User.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserUseCase(IUserWriteOnlyRepository userWriteOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository, IUnitOfWork unitOfWork)
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid userId, RequestUpdateUserJson request)
    {
        ValidateAndThrowOnFailure(request);

        var user = await _userReadOnlyRepository.FindByIdAsync(userId);

        if (user is null)
            throw new NotFoundErrorException("User not found.");

        user.Update(request.Name);

        _userWriteOnlyRepository.Update(user);

        await _unitOfWork.CommitAsync();
    }

    private void ValidateAndThrowOnFailure(RequestUpdateUserJson request)
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