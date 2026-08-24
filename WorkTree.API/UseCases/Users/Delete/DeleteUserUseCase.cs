using WorkTree.API.Contracts.Repositories;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.API.UseCases.Users.Delete;

public class DeleteUserUseCase
{
    private readonly IUserRepository _userRepository;

    public DeleteUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void Execute(Guid userId)
    {
        var user = _userRepository.FindById(userId);

        if (user is null)
            throw new NotFoundErrorException("User does not exist");

        _userRepository.Delete(user);
    }
}