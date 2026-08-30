using FluentValidation;
using WorkTree.Communication.Requests;

namespace WorkTree.Application.UseCases.Tenant.Update;

public class RequestUpdateTenantValidator : AbstractValidator<RequestUpdateTenantJson>
{
    public RequestUpdateTenantValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage("Name could not be empty.");
    }
}