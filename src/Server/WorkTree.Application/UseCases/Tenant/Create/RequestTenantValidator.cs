using FluentValidation;
using WorkTree.Communication.Requests.Tenants;
using WorkTree.Exceptions;

namespace WorkTree.Application.UseCases.Tenant.Create;

public class RequestTenantValidator : AbstractValidator<RequestTenantJson>
{
    public RequestTenantValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED);

        When(user => !string.IsNullOrWhiteSpace(user.Email),
            () =>
            {
                RuleFor(user => user.Email).EmailAddress()
                    .WithMessage(ResourceMessagesException.VALIDATION_EMAIL_INVALID);
            });
    }
}