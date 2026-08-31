using WorkTree.Application.UseCases.User.Create;
using WorkTree.Communication.Requests.Users;

namespace Validators.Tests.User.Create;

public class CreateUserValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = new RequestUserJson
        {
            Name = "John Doe",
            Email = "johndoe@gmail.com",
            Password = "123456",
            TenantId = Guid.NewGuid(),
        };

        var validator = new CreateUserValidator();

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }
}