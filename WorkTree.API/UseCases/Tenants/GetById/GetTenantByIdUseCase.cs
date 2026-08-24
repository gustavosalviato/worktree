using WorkTree.API.Contracts.Repositories;
using WorkTree.Communication.Responses.Tenants;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.API.UseCases.Tenants.GetById;

public class GetTenantByIdUseCase
{
    private readonly ITenantRepository _tenantRepository;

    public GetTenantByIdUseCase(ITenantRepository tenantRepository) => _tenantRepository = tenantRepository;


    public async Task<ResponseTenantJson> Execute(Guid tenantId)
    {
        var tenant = await _tenantRepository.FindByIdAsync(tenantId);

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