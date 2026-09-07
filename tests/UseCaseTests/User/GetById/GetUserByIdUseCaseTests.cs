using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using Shouldly;
using WorkTree.Application.UseCases.User.GetById;

namespace UseCaseTests.User.GetById;

public class GetUserByIdUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();


        var useCase = CreateUseCase(user);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Email.ShouldBe(user.Email);
    }


    private static GetUserByIdUseCase CreateUseCase(WorkTree.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetUserByIdUseCase(loggedUser);
    }
}