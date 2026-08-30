namespace WorkTree.Application.UseCases.User.Delete;

public interface IDeleteUserUseCase
{
    Task Execute(Guid userId);
}