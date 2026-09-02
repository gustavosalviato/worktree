using FluentValidation;
using WorkTree.Communication.Requests.Users;
using WorkTree.Exceptions;

namespace WorkTree.Application.UseCases.User.Create;

public class CreateUserValidator : AbstractValidator<RequestCreateUserJson>
{
    public CreateUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED);
        RuleFor(user => user.Password).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED);
        
        RuleFor(user => user.TenantId).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_TENANT_REQUIRED);

        When(user => !string.IsNullOrWhiteSpace(user.Email),
            () =>
            {
                RuleFor(user => user.Email).EmailAddress()
                    .WithMessage(ResourceMessagesException.VALIDATION_EMAIL_INVALID);
            });

        When(user => !string.IsNullOrWhiteSpace(user.Password),
            () =>
            {
                RuleFor(user => user.Password).MinimumLength(8)
                    .WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_MINIMUM_LENGTH);
            });
    }
}