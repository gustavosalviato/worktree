using WorkTree.Communication.Requests;
using WorkTree.Domain.Identity;
using WorkTree.Domain.Repositories;
using WorkTree.Domain.Repositories.Tenant;
using WorkTree.Exceptions;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.Application.UseCases.Tenant.Update;

public class UpdateTenantUseCase : IUpdateTenantUseCase
{
    private readonly ITenantUpdateOnlyRepository _tenantUpdateOnlyRepository;
    private readonly ITenantReadOnlyRepository _tenantReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public UpdateTenantUseCase
    (
        ITenantUpdateOnlyRepository tenantUpdateOnlyRepository,
        ITenantReadOnlyRepository tenantReadOnlyRepository,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork
    )
    {
        _tenantUpdateOnlyRepository = tenantUpdateOnlyRepository;
        _tenantReadOnlyRepository = tenantReadOnlyRepository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }

    public async Task Execute(RequestUpdateTenantJson request)
    {
        Validate(request);

        var user = await _loggedUser.Get();

        var tenant = await _tenantReadOnlyRepository.FindByIdAsync(user.TenantId);

        if (tenant is null)
            throw new NotFoundErrorException(ResourceMessagesException.ORGANIZATION_NOT_FOUND);

        tenant.Update(request.Name);

        _tenantUpdateOnlyRepository.Update(tenant);

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