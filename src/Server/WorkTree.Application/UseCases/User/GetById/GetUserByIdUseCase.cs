using Mapster;
using WorkTree.Communication.Responses.Users;
using WorkTree.Domain.Identity;

namespace WorkTree.Application.UseCases.User.GetById;

public class GetUserByIdUseCase : IGetUserByIdUseCase
{
    private readonly ILoggedUser _loggedUser;

    public GetUserByIdUseCase(ILoggedUser loggedUser)
    {
        _loggedUser = loggedUser;
    }

    public async Task<ResponseUserJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        return loggedUser.Adapt<ResponseUserJson>();
    }
}