using WorkTree.API.Contracts.Repositories;
using WorkTree.Communication.Responses.Users;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.API.UseCases.Users.GetById;

public class GetUserByIdUseCase
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResponseUserJson> Execute(Guid userId)
    {
        var user = await _userRepository.FindByIdAsync(userId);

        if (user is null)
            throw new NotFoundErrorException("User does not exist");

        return new ResponseUserJson
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            TenantId = user.TenantId
        };
    }
}