using WorkTree.Domain.Repositories;
using WorkTree.Domain.Repositories.Tenant;
using WorkTree.Exceptions;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.Application.UseCases.Tenant.Delete;

public class DeleteTenantUseCase : IDeleteTenantUseCase
{
    private readonly ITenantReadOnlyRepository _tenantReadOnlyRepository;
    private readonly ITenantWriteOnlyRepository _tenantWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;


    public DeleteTenantUseCase(ITenantReadOnlyRepository tenantReadOnlyRepository,
        ITenantWriteOnlyRepository tenantWriteOnlyRepository, IUnitOfWork unitOfWork)
    {
        _tenantReadOnlyRepository = tenantReadOnlyRepository;
        _tenantWriteOnlyRepository = tenantWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task Execute(Guid tenantId)
    {
        var tenant = await _tenantReadOnlyRepository.FindByIdAsync(tenantId);

        if (tenant is null)
            throw new NotFoundErrorException(ResourceMessagesException.ORGANIZATION_NOT_FOUND);

        _tenantWriteOnlyRepository.Delete(tenant);

        await _unitOfWork.CommitAsync();
    }
}