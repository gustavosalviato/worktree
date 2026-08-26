using FluentValidation;
using WorkTree.Communication.Requests;

namespace WorkTree.API.UseCases.Tenants.Update;

public class RequestUpdateTenantValidator : AbstractValidator<RequestUpdateTenantJson>
{
    public RequestUpdateTenantValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage("Name could not be empty.");
    }
}