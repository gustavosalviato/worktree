using WorkTree.Communication.Requests.Users;

namespace WorkTree.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}