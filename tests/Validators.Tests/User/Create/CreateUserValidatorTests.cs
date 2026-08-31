using CommonTestUtilities.Requests;
using Shouldly;
using WorkTree.Application.UseCases.User.Create;
using WorkTree.Communication.Requests.Users;

namespace Validators.Tests.User.Create;

public class CreateUserValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RequestCreateUserJsonBuilder.Build();

        var validator = new CreateUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }
}