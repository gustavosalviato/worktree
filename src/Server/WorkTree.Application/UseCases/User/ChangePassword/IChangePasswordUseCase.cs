using WorkTree.Communication.Requests.Users;

namespace WorkTree.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}