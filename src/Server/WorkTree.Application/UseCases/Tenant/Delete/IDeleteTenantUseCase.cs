namespace WorkTree.Application.UseCases.Tenant.Delete;

public interface IDeleteTenantUseCase
{
    Task Execute(Guid tenantId);
}