using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using Shouldly;
using WorkTree.Application.UseCases.User.ChangePassword;
using WorkTree.Exceptions;
using WorkTree.Exceptions.ExceptionsBase;

namespace UseCaseTests.User.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        request.CurrentPassword = user.PasswordHash;

        var useCase = CreateUseCase(user, request.CurrentPassword);

        await useCase.Execute(request);
    }

    [Fact]
    public async Task ShouldThrowException_WhenCurrentPasswordDoesNotMatch()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();
        var useCase = CreateUseCase(user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidCredentialsException>();

        exception.GetErrors().ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(ResourceMessagesException.VALIDATION_CURRENT_PASSWORD)
        ]);
    }


    private static ChangePasswordUseCase CreateUseCase(WorkTree.Domain.Entities.User user, string? password = null)
    {
        var passwordHasher = new PasswordHasherBuilder();
        var loggedUser = LoggedUserBuilder.Build(user);
        var updateOnlyRepository = UserUpdateOnlyRepositoryBuilder.Build();

        if (password is not null)
            passwordHasher.VerifyPassword(password);


        return new ChangePasswordUseCase(loggedUser, passwordHasher.Build(), updateOnlyRepository);
    }
}