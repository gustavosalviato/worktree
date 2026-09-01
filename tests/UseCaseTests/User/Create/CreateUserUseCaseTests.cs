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
        var useCase = CreateUseCase(null, request.TenantId);

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

        var useCase = CreateUseCase(null, request.TenantId);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrors().ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain("Name could not be empty.")
        ]);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var request = RequestCreateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(request.Email, request.TenantId);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ConflictErrorException>();

        exception.GetErrors().ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain("User with this email already exists.")
        ]);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenTenantIdIsEmptyOrDoesNotExist()
    {
        var request = RequestCreateUserJsonBuilder.Build();
        request.TenantId = Guid.Empty;

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request).ShouldThrowAsync<NotFoundErrorException>();
        exception.GetErrors().ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain("Tenant not found."),
        ]);
    }


    private CreateUserUseCase CreateUseCase
    (
        string? emailThatAlreadyExists = null,
        Guid? tenantIdThatAlreadyExists = null
    )
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordHasher = new PasswordHasherBuilder().Build();
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var tenantReadOnlyRepositoryBuilder = new TenantReadOnlyRepositoryBuilder();

        if (emailThatAlreadyExists is not null)
            userReadOnlyRepository.FindByEmailAsync(emailThatAlreadyExists);


        if (tenantIdThatAlreadyExists is not null)
            tenantReadOnlyRepositoryBuilder.FindByIdAsync(tenantIdThatAlreadyExists.Value);

        return new CreateUserUseCase(
            passwordHasher,
            userWriteOnlyRepository,
            userReadOnlyRepository.Build(),
            tenantReadOnlyRepositoryBuilder.Build(),
            unitOfWork
        );
    }
}