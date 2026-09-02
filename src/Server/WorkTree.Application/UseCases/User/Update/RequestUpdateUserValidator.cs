using FluentValidation;
using WorkTree.Communication.Requests.Users;
using WorkTree.Exceptions;

namespace WorkTree.Application.UseCases.User.Update;

public class RequestUpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public RequestUpdateUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
    }
}