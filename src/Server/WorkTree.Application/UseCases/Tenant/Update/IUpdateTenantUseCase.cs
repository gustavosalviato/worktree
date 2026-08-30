using WorkTree.Communication.Requests;

namespace WorkTree.Application.UseCases.Tenant.Update;

public interface IUpdateTenantUseCase
{
    Task Execute(Guid tenantId, RequestUpdateTenantJson request);
}