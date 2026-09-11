using FluentValidation;
using WorkTree.Exceptions;

namespace WorkTree.Application.UseCases.Shared.Validators;

public static class PasswordValidator
{
    extension<TRequest>(IRuleBuilderInitial<TRequest, string> ruleBuilder)
    {
        internal IRuleBuilderOptions<TRequest, string> Password()
        {
            return ruleBuilder.Cascade(CascadeMode.Stop).NotEmpty()
                .WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED)
                .MinimumLength(8).WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_MINIMUM_LENGTH);
        }
    }
}