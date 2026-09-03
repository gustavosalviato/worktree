using WorkTree.Communication.Requests;
using WorkTree.Domain.Repositories;
using WorkTree.Domain.Repositories.Tenant;
using WorkTree.Exceptions;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.Application.UseCases.Tenant.Update;

public class UpdateTenantUseCase : IUpdateTenantUseCase
{
    private readonly ITenantWriteOnlyRepository _tenantWriteOnlyRepository;
    private readonly ITenantReadOnlyRepository _tenantReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTenantUseCase(ITenantWriteOnlyRepository tenantWriteOnlyRepository,
        ITenantReadOnlyRepository tenantReadOnlyRepository, IUnitOfWork unitOfWork)
    {
        _tenantWriteOnlyRepository = tenantWriteOnlyRepository;
        _tenantReadOnlyRepository = tenantReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid tenantId, RequestUpdateTenantJson request)
    {
        Validate(request);

        var tenant = await _tenantReadOnlyRepository.FindByIdAsync(tenantId);

        if (tenant is null)
            throw new NotFoundErrorException(ResourceMessagesException.ORGANIZATION_NOT_FOUND);

        tenant.Update(request.Name);

        _tenantWriteOnlyRepository.Update(tenant);

        await _unitOfWork.CommitAsync();
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