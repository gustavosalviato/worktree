using Mapster;
using WorkTree.Communication.Requests.Tenants;
using WorkTree.Communication.Responses.Tenants;
using WorkTree.Domain.Repositories;
using WorkTree.Domain.Repositories.Tenant;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.Application.UseCases.Tenant.Create;

public class CreateTenantUseCase : ICreateTenantUseCase
{
    private readonly ITenantWriteOnlyRepository _tenantWriteOnlyRepository;
    private readonly ITenantReadOnlyRepository _tenantReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTenantUseCase(ITenantWriteOnlyRepository tenantWriteOnlyRepository,
        ITenantReadOnlyRepository tenantReadOnlyRepository, IUnitOfWork unitOfWork)
    {
        _tenantWriteOnlyRepository = tenantWriteOnlyRepository;
        _tenantReadOnlyRepository = tenantReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseTenantJson> Execute(RequestTenantJson request)
    {
        await ValidateAndThrowFailure(request);

        var tenant = request.Adapt<Domain.Entities.Tenant>();

        await _tenantWriteOnlyRepository.AddAsync(tenant);

        await _unitOfWork.CommitAsync();

        return new ResponseTenantJson
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Email = tenant.Email,
        };
    }


    private async Task ValidateAndThrowFailure(RequestTenantJson request)
    {
        var validator = new RequestTenantValidator();

        var result = await validator.ValidateAsync(request);

        var exists = await _tenantReadOnlyRepository.FindByEmailAsync(request.Email);

        if (exists is not null)
            throw new ConflictErrorException("A tenant with this email already exists.");

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}