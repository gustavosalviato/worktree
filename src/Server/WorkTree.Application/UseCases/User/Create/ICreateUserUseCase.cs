using WorkTree.Communication.Requests.Users;
using WorkTree.Communication.Responses.Users;

namespace WorkTree.Application.UseCases.User.Create;

public interface ICreateUserUseCase
{
    Task<ResponseUserJson> Execute(RequestCreateUserJson requestCreate);
}