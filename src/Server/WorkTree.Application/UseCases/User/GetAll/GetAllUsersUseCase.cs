using WorkTree.Communication.Responses.Users;
using WorkTree.Domain.Repositories.User;

namespace WorkTree.Application.UseCases.User.GetAll;

public class GetAllUsersUseCase : IGetAllUsersUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public GetAllUsersUseCase(IUserWriteOnlyRepository userWriteOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository)
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task<List<ResponseUserJson>> Execute()
    {
        var users = await _userReadOnlyRepository.FindManyAsync();

        return users.Select(user => new ResponseUserJson
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            TenantId = user.TenantId
        }).ToList();
    }
}