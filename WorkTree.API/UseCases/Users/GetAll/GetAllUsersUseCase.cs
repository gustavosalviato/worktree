using WorkTree.API.Contracts.Repositories;
using WorkTree.Communication.Responses.Users;

namespace WorkTree.API.UseCases.Users.GetAll;

public class GetAllUsersUseCase
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public List<ResponseUserJson> Execute()
    {
        var users = _userRepository.FindMany();

        return users.Select(user => new ResponseUserJson
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            TenantId = user.TenantId
        }).ToList();
    }
}