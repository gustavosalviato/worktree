using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using Shouldly;
using WorkTree.Application.UseCases.User.Create;
using WorkTree.Domain.Entities;
using WorkTree.Exceptions;
using WorkTree.Exceptions.ExceptionsBase;

namespace UseCaseTests.User.Create;

public class CreateUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var tenant = TenantBuilder.Build();
        var request = RequestCreateUserJsonBuilder.Build();

        request.TenantId = tenant.Id;

        var useCase = CreateUseCase(null, tenant);

        var result = await useCase.Execute(request);


        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Email.ShouldBe(request.Email);
        result.TenantId.ShouldBe(request.TenantId);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenNameIsEmpty()
    {
        var tenant = TenantBuilder.Build();
        var request = RequestCreateUserJsonBuilder.Build();

        request.Name = string.Empty;
        request.TenantId = tenant.Id;

        var useCase = CreateUseCase(tenant: tenant);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrors().ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(ResourceMessagesException.VALIDATION_NAME_REQUIRED)
        ]);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var user = UserBuilder.Build();
        var tenant = TenantBuilder.Build();

        var request = RequestCreateUserJsonBuilder.Build();

        request.Email = user.Email;
        request.TenantId = tenant.Id;

        var useCase = CreateUseCase(user, tenant);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ConflictErrorException>();

        exception.GetErrors().ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(ResourceMessagesException.USER_WITH_EMAIL_ALREADY_EXISTS)
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
            e => e.ShouldContain(ResourceMessagesException.ORGANIZATION_NOT_FOUND),
        ]);
    }


    private static CreateUserUseCase CreateUseCase
    (
        WorkTree.Domain.Entities.User? user = null,
        Tenant? tenant = null
    )
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordHasher = new PasswordHasherBuilder().Build();
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var tenantReadOnlyRepositoryBuilder = new TenantReadOnlyRepositoryBuilder();

        if (user is not null)
            userReadOnlyRepository.FindByEmailAsync(user);


        if (tenant is not null)
            tenantReadOnlyRepositoryBuilder.FindByIdAsync(tenant);

        return new CreateUserUseCase(
            passwordHasher,
            userWriteOnlyRepository,
            userReadOnlyRepository.Build(),
            tenantReadOnlyRepositoryBuilder.Build(),
            unitOfWork
        );
    }
}