using WorkTree.API.Contracts.Repositories;
using WorkTree.Communication.Requests;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.API.UseCases.Tenants.Update;

public class UpdateTenantUseCase
{
    private readonly ITenantRepository _tenantRepository;

    public UpdateTenantUseCase(ITenantRepository tenantRepository) => _tenantRepository = tenantRepository;

    public async Task Execute(Guid tenantId, RequestUpdateTenantJson request)
    {
        Validate(request);

        var tenant = await _tenantRepository.FindByIdAsync(tenantId);

        if (tenant is null)
            throw new NotFoundErrorException("Tenant not found.");

        tenant.Name = request.Name;
        tenant.Touch();

        await _tenantRepository.UpdateAsync(tenant);
    }

    private void Validate(RequestUpdateTenantJson request)
    {
        var validator = new RequestUpdateTenantValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}