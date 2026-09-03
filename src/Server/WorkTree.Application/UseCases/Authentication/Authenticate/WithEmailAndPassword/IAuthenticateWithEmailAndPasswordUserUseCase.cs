using WorkTree.Communication.Requests.Auth;
using WorkTree.Communication.Responses.Auth;

namespace WorkTree.Application.UseCases.Authentication.Authenticate.WithEmailAndPassword;

public interface IAuthenticateWithEmailAndPasswordUserUseCase
{
    Task<ResponseAuthenticateUserJson> Execute(RequestAuthenticateJson request);
}