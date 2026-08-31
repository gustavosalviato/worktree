using FluentValidation;
using WorkTree.Communication.Requests.Users;

namespace WorkTree.Application.UseCases.User.Create;

public class RequestUserValidator : AbstractValidator<RequestUserJson>
{
   public RequestUserValidator()
   {
      RuleFor(user => user.Name).NotEmpty().WithMessage("Name could not be empty.");
      RuleFor(user => user.Email).EmailAddress().WithMessage("Invalid email address.");
      RuleFor(user => user.Password).NotEmpty().WithMessage("Password could not be empty").MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
      RuleFor(user => user.TenantId).NotEmpty().WithMessage("tenantId could not be empty.");
   }
}