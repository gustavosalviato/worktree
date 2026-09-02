using FluentValidation;
using WorkTree.Communication.Requests;
using WorkTree.Exceptions;

namespace WorkTree.Application.UseCases.Tenant.Update;

public class RequestUpdateTenantValidator : AbstractValidator<RequestUpdateTenantJson>
{
    public RequestUpdateTenantValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
    }
}