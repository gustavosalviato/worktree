using CommonTestUtilities.Requests;
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

        Assert.True(result.IsValid);
    }
}