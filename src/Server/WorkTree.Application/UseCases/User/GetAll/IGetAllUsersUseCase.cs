using WorkTree.Communication.Responses.Users;

namespace WorkTree.Application.UseCases.User.GetAll;

public interface IGetAllUsersUseCase
{
    Task<List<ResponseUserJson>> Execute();
}