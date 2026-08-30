using WorkTree.Communication.Responses.Tenants;

namespace WorkTree.Application.UseCases.Tenant.GetById;

public interface IGetTenantByIdUseCase
{
    Task<ResponseTenantJson> Execute(Guid tenantId);
}