using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Shouldly;
using WorkTree.Application.UseCases.User.Update;
using WorkTree.Exceptions;
using WorkTree.Exceptions.ExceptionsBase;

namespace UseCaseTests.User.Update;

public class UpdateUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        await useCase.Execute(request);

        user.Name.ShouldBe(request.Name);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenNameIsEmpty()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrors().ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(ResourceMessagesException.VALIDATION_NAME_REQUIRED)
        ]);
    }


    private static UpdateUserUseCase CreateUseCase(WorkTree.Domain.Entities.User user)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var updateOnlyRepository = UserUpdateOnlyRepositoryBuilder.Build();


        return new UpdateUserUseCase(unitOfWork, loggedUser, updateOnlyRepository);
    }
}