using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using Shouldly;
using WorkTree.Application.UseCases.User.GetAll;

namespace UseCaseTests.User.GetAll;

public class GetAllUsersUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var tenantId = Guid.NewGuid();
        var (loggedUser, _) = UserBuilder.Build();
        loggedUser.ChangeTenantId(tenantId);

        var (firstUser, _) = UserBuilder.Build();
        firstUser.ChangeTenantId(tenantId);

        var (secondUser, _) = UserBuilder.Build();
        secondUser.ChangeTenantId(tenantId);

        var users = new List<WorkTree.Domain.Entities.User> { firstUser, secondUser };

        var useCase = CreateUseCase(loggedUser, tenantId, users);

        var result = await useCase.Execute();

        result.Count.ShouldBe(2);

        result[0].Id.ShouldBe(firstUser.Id);
        result[0].Name.ShouldBe(firstUser.Name);
        result[0].Email.ShouldBe(firstUser.Email);
        result[0].TenantId.ShouldBe(tenantId);

        result[1].Id.ShouldBe(secondUser.Id);
        result[1].Name.ShouldBe(secondUser.Name);
        result[1].Email.ShouldBe(secondUser.Email);
        result[1].TenantId.ShouldBe(tenantId);
    }

    [Fact]
    public async Task Success_WhenNoUsersInTenant()
    {
        var tenantId = Guid.NewGuid();
        var (loggedUser, _) = UserBuilder.Build();
        loggedUser.ChangeTenantId(tenantId);

        var useCase = CreateUseCase(loggedUser, tenantId, []);

        var result = await useCase.Execute();

        result.ShouldBeEmpty();
    }

    private static GetAllUsersUseCase CreateUseCase(
        WorkTree.Domain.Entities.User loggedUser,
        Guid tenantId,
        List<WorkTree.Domain.Entities.User> users)
    {
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
        userReadOnlyRepository.FindManyByTenantIdAsync(tenantId, users);

        return new GetAllUsersUseCase(userReadOnlyRepository.Build(), LoggedUserBuilder.Build(loggedUser));
    }
}