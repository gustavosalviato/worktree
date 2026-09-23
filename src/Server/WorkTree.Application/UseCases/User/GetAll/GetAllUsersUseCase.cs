using WorkTree.Communication.Responses.Users;
using WorkTree.Domain.Identity;
using WorkTree.Domain.Repositories.User;

namespace WorkTree.Application.UseCases.User.GetAll;

public class GetAllUsersUseCase : IGetAllUsersUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly ILoggedUser _loggedUser;

    public GetAllUsersUseCase(IUserReadOnlyRepository userReadOnlyRepository, ILoggedUser loggedUser)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _loggedUser = loggedUser;
    }

    public async Task<List<ResponseUserJson>> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var users = await _userReadOnlyRepository.FindManyByTenantIdAsync(loggedUser.TenantId);

        return users.Select(user => new ResponseUserJson
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            TenantId = user.TenantId
        }).ToList();
    }
}