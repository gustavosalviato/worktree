using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using Shouldly;
using WorkTree.Application.UseCases.User.Delete;
using WorkTree.Exceptions;
using WorkTree.Exceptions.ExceptionsBase;

namespace UseCaseTests.User.Delete;

public class DeleteUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        await useCase.Execute(user.Id);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenUserDoestNotExist()
    {
        var useCase = CreateUseCase();

        var exception = await useCase.Execute(Guid.NewGuid()).ShouldThrowAsync<NotFoundErrorException>();

        exception.GetErrors().ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(ResourceMessagesException.USER_NOT_FOUND),
        ]);
    }


    private static DeleteUserUseCase CreateUseCase(WorkTree.Domain.Entities.User? user = null)
    {
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if (user is not null)
            userReadOnlyRepository.FindByIdAsync(user);

        return new DeleteUserUseCase(userWriteOnlyRepository, userReadOnlyRepository.Build(), unitOfWork);
    }
}