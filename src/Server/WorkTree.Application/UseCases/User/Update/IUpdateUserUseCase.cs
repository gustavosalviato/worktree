using WorkTree.Communication.Requests.Users;

namespace WorkTree.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    Task Execute(Guid userId, RequestUpdateUserJson request);
}