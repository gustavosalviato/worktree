using FluentValidation;
using WorkTree.Application.UseCases.Shared.Validators;
using WorkTree.Communication.Requests.Users;

namespace WorkTree.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(request => request.NewPassword).Password();
    }
}