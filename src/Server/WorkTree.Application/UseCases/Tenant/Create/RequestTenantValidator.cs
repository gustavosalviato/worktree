using FluentValidation;
using WorkTree.Communication.Requests.Tenants;

namespace WorkTree.Application.UseCases.Tenant.Create;

public class RequestTenantValidator : AbstractValidator<RequestTenantJson>
{
    public RequestTenantValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage("Name could not be empty.");
        RuleFor(user => user.Email).EmailAddress().WithMessage("Invalid email address.");
    }
}