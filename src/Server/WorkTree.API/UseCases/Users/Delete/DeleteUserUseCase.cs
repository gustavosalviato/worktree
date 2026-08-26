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

    public async Task Execute(Guid userId)
    {
        var user = await _userRepository.FindByIdAsync(userId);

        if (user is null)
            throw new NotFoundErrorException("User does not exist");

        await _userRepository.DeleteAsync(user);
    }
}