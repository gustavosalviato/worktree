using WorkTree.Communication.Responses.Tenants;
using WorkTree.Domain.Repositories.Tenant;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.Application.UseCases.Tenant.GetById;

public class GetTenantByIdUseCase : IGetTenantByIdUseCase
{
    private readonly ITenantReadOnlyRepository _tenantReadOnlyRepository;

    public GetTenantByIdUseCase(ITenantReadOnlyRepository tenantReadOnlyRepository) =>
        _tenantReadOnlyRepository = tenantReadOnlyRepository;


    public async Task<ResponseTenantJson> Execute(Guid tenantId)
    {
        var tenant = await _tenantReadOnlyRepository.FindByIdAsync(tenantId);

        if (tenant is null)
            throw new NotFoundErrorException("Tenant not found");


        return new ResponseTenantJson
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Email = tenant.Email
        };
    }
}