using FluentValidation;
using WorkTree.Communication.Requests.Users;

namespace WorkTree.Application.UseCases.User.Update;

public class RequestUpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public RequestUpdateUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage("Name could not be empty.");
    }
}