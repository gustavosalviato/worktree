using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Shouldly;
using WorkTree.Application.UseCases.Tenant.Create;
using WorkTree.Communication.Requests.Tenants;
using WorkTree.Exceptions;
using WorkTree.Exceptions.ExceptionsBase;

namespace UseCaseTests.Tenant.Create;

public class CreateTenantUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var request = RequestCreateTenantJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Email.ShouldBe(request.Email);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var tenant = TenantBuilder.Build();

        var request = new RequestCreateTenantJson
        {
            Email = tenant.Email,
            Name = tenant.Name
        };

        var useCase = CreateUseCase(tenant);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ConflictErrorException>();

        exception.GetErrors().ShouldSatisfy(
        [
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(ResourceMessagesException.ORGANIZATION_WITH_EMAIL_ALREADY_EXISTS),
        ]);
    }

    private static CreateTenantUseCase CreateUseCase(WorkTree.Domain.Entities.Tenant? tenant = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var tenantWriteOnlyRepository = TenantWriteOnlyRepositoryBuilder.Build();
        var tenantReadOnlyRepository = new TenantReadOnlyRepositoryBuilder();

        if (tenant is not null)
            tenantReadOnlyRepository.FindByEmailAsync(tenant);

        return new CreateTenantUseCase(tenantWriteOnlyRepository, tenantReadOnlyRepository.Build(), unitOfWork);
    }
}