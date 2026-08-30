using WorkTree.Domain.Repositories;
using WorkTree.Domain.Repositories.User;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.Application.UseCases.User.Delete;

public class DeleteUserUseCase : IDeleteUserUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserUseCase(IUserWriteOnlyRepository userWriteOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository, IUnitOfWork unitOfWork)
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid userId)
    {
        var user = await _userReadOnlyRepository.FindByIdAsync(userId);

        if (user is null)
            throw new NotFoundErrorException("User does not exist");

        _userWriteOnlyRepository.Delete(user);

        await _unitOfWork.CommitAsync();
    }
}