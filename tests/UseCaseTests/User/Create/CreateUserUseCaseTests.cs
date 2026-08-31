using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using WorkTree.Application.UseCases.User.Create;

namespace UseCaseTests.User.Create;

public class CreateUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var request = RequestCreateUserJsonBuilder.Build();
    }

    private CreateUserUseCase CreateUseCase()
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder().Build();
        var passwordHasher = new PasswordHasherBuilder().Build();

        return new CreateUserUseCase(passwordHasher, userWriteOnlyRepository, userReadOnlyRepository, unitOfWork);
    }
}