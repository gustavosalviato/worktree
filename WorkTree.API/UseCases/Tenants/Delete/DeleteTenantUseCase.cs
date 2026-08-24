using WorkTree.API.Contracts.Repositories;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.API.UseCases.Tenants.Delete;

public class DeleteTenantUseCase
{
    private readonly ITenantRepository _tenantRepository;


    public DeleteTenantUseCase(ITenantRepository tenantRepository) => _tenantRepository = tenantRepository;


    public async Task Execute(Guid tenantId)
    {
        var tenant = await _tenantRepository.FindByIdAsync(tenantId);

        if (tenant is null)
            throw new NotFoundErrorException("Tenant not found");

        await _tenantRepository.DeleteAsync(tenant);
    }
}