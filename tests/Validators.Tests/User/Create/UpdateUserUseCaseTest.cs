using CommonTestUtilities.Requests;
using Shouldly;
using WorkTree.Application.UseCases.User.Update;
using WorkTree.Exceptions;

namespace Validators.Tests.User.Create;

public class UpdateUserUseCaseTest
{
    [Fact]
    public void Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var validator = new RequestUpdateUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Validation_ShouldHaveError_WhenNameIsEmpty(string name)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = name;

        var validator = new RequestUpdateUserValidator();

        var result = validator.Validate(request);

        result.Errors.ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_NAME_REQUIRED)),
        ]);
    }
 
}