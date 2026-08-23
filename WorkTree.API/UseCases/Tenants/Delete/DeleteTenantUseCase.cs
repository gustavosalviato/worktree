using WorkTree.API.Contracts;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.API.UseCases.Tenants.Delete;

public class DeleteTenantUseCase
{
    private readonly ITenantRepository _tenantRepository;


    public DeleteTenantUseCase(ITenantRepository tenantRepository) => _tenantRepository = tenantRepository;


    public void Execute(Guid tenantId)
    {
        var tenant = _tenantRepository.FindById(tenantId);

        if (tenant is null)
            throw new NotFoundErrorException("Tenant not found");

        _tenantRepository.Delete(tenant);
    }
}