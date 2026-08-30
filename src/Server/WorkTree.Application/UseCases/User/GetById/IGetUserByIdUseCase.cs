using WorkTree.Communication.Responses.Users;

namespace WorkTree.Application.UseCases.User.GetById;

public interface IGetUserByIdUseCase
{
    Task<ResponseUserJson> Execute(Guid userId);
}