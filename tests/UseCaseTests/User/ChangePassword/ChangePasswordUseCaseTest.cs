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
        var (user, password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        request.CurrentPassword = password;

        var useCase = CreateUseCase(user, password);

        await useCase.Execute(request);
    }

    [Fact]
    public async Task ShouldThrowException_WhenNewPasswordIsEmpty()
    {
        var (user, password) = UserBuilder.Build();
        
        var request = RequestChangePasswordJsonBuilder.Build();
        
        request.NewPassword = string.Empty;
        request.CurrentPassword = password;
        
        var useCase = CreateUseCase(user, password);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrors().ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED)
        ]);
    }

    [Fact]
    public async Task ShouldThrowException_WhenCurrentPasswordDoesNotMatch()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();
        var useCase = CreateUseCase(user, "invalid-password");

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidCredentialsException>();

        exception.GetErrors().ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(ResourceMessagesException.VALIDATION_CURRENT_PASSWORD)
        ]);
    }


    private static ChangePasswordUseCase CreateUseCase(WorkTree.Domain.Entities.User user, string password)
    {
        
        var loggedUser = LoggedUserBuilder.Build(user);
        var updateOnlyRepository = UserUpdateOnlyRepositoryBuilder.Build();

        var passwordHasher = new PasswordHasherBuilder().VerifyPassword(password);


        return new ChangePasswordUseCase(loggedUser, passwordHasher.Build(), updateOnlyRepository);
    }
}