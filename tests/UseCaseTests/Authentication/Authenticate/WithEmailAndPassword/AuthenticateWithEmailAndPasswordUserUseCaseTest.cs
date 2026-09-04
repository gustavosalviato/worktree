using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using Shouldly;
using WorkTree.Application.UseCases.Authentication.Authenticate.WithEmailAndPassword;
using WorkTree.Exceptions;
using WorkTree.Exceptions.ExceptionsBase;

namespace UseCaseTests.Authentication.Authenticate.WithEmailAndPassword;

public class AuthenticateWithEmailAndPasswordUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestAuthenticateJsonBuilder.Build();

        request.Email = user.Email;
        request.Password = user.PasswordHash;


        var useCase = CreateUseCase(user.PasswordHash, user);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.AccessToken.ShouldNotBeNull();
        result.RefreshToken.ShouldNotBeNull();
    }

    [Fact]
    public async Task ShouldThrowException_WhenUserDoesNotExist()
    {
        var request = RequestAuthenticateJsonBuilder.Build();

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidCredentialsException>();


        exception.GetErrors().ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(ResourceMessagesException.INVALID_CREDENTIALS)
        ]);
    }

    [Fact]
    public async Task ShouldThrowException_WhenPasswordDoesNotMatch()
    {
        var (user, password) = UserBuilder.Build();
        var request = RequestAuthenticateJsonBuilder.Build();

        request.Email = user.Email;

        var useCase = CreateUseCase(user: user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidCredentialsException>();

        exception.GetErrors().ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(ResourceMessagesException.INVALID_CREDENTIALS)
        ]);
    }

    private AuthenticateWithEmailAndPasswordUserUseCase CreateUseCase(string? password = null,
        WorkTree.Domain.Entities.User? user = null)
    {
        var passwordHasherBuilder = new PasswordHasherBuilder();
        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var accessTokenGeneratorBuilder = AccessTokenGeneratorBuilder.Build();

        if (user is not null)
            userReadOnlyRepositoryBuilder.FindByEmailAsync(user);


        if (password is not null)
            passwordHasherBuilder.VerifyPassword(password);


        return new AuthenticateWithEmailAndPasswordUserUseCase
        (
            userReadOnlyRepositoryBuilder.Build(),
            passwordHasherBuilder.Build(),
            accessTokenGeneratorBuilder
        );
    }
}