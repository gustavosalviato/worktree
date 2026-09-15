using CommonTestUtilities.Requests;
using Shouldly;
using WorkTree.Application.UseCases.User.ChangePassword;
using WorkTree.Exceptions;

namespace Validators.Tests.User.ChangePassword;

public class ChangePasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var validator = new ChangePasswordValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Validation_ShouldHaveError_WhenPasswordIsIsEmpty()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;

        var validator = new ChangePasswordValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED)),
        ]);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    public void Validation_ShouldHaveError_WhenNewPasswordIsTooShort(int passwordLength)
    {
        var request = RequestChangePasswordJsonBuilder.Build(passwordLength);

        request.NewPassword = request.NewPassword[..5];

        var validator = new ChangePasswordValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_PASSWORD_MINIMUM_LENGTH)),
        ]);
    }
}