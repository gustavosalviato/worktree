using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using Shouldly;
using WorkTree.Application.UseCases.User.Create;
using WorkTree.Exceptions.ExceptionsBase;

namespace UseCaseTests.User.Create;

public class CreateUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var request = RequestCreateUserJsonBuilder.Build();
        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);


        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Email.ShouldBe(request.Email);
        result.TenantId.ShouldBe(request.TenantId);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenNameIsEmpty()
    {
        var request = RequestCreateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();
        exception.GetErrors().ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain("Name could not be empty.");
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var request = RequestCreateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(request.Email);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ConflictErrorException>();
        exception.GetErrors().ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain("User with this email already exists.");
        });
    }


    private CreateUserUseCase CreateUseCase(string? emailThatAlreadyExists = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordHasher = new PasswordHasherBuilder().Build();
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();

        if (emailThatAlreadyExists is not null)
        {
            userReadOnlyRepository.FindByEmailAsync(emailThatAlreadyExists);
        }

        return new CreateUserUseCase(passwordHasher, userWriteOnlyRepository, userReadOnlyRepository.Build(),
            unitOfWork);
    }
}