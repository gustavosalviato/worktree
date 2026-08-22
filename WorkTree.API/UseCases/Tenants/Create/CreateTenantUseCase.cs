using WorkTree.API.Contracts;
using WorkTree.API.UseCases.Tenant.Create;
using WorkTree.Communication.Requests;
using WorkTree.Communication.Responses.Tenant;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.API.UseCases.Tenants.Create;

public class CreateTenantUseCase
{
    private readonly ITenantRepository _tenantRepository;

    public CreateTenantUseCase(ITenantRepository tenantRepository) => _tenantRepository = tenantRepository;

    public ResponseTenantJson Execute(RequestTenantJson request)
    {
        Validate(request);

        var exists = _tenantRepository.FindByEmail(request.Email);

        if (exists is not null)
            throw new ConflictErrorException("User with this email already exists.");


        var tenant = new Entities.Tenant
        {
            Name = request.Name,
            Email = request.Email,
        };
        
        _tenantRepository.Create(tenant);

        return new ResponseTenantJson
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Email = tenant.Email,
        };
    }


    private void Validate(RequestTenantJson request)
    {
        var validator = new RequestTenantValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}