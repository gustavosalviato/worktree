using WorkTree.Communication.Requests.Tenants;
using WorkTree.Communication.Responses.Tenants;

namespace WorkTree.Application.UseCases.Tenant.Create;

public interface ICreateTenantUseCase
{
    Task<ResponseTenantJson> Execute(RequestTenantJson request);
}