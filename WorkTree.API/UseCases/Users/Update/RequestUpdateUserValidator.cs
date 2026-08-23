using WorkTree.Communication.Requests;
using FluentValidation;
using WorkTree.Communication.Requests.Users;

namespace WorkTree.API.UseCases.Users.Update;

public class RequestUpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public RequestUpdateUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage("Name could not be empty.");
    }
}