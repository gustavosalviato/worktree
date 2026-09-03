using CommonTestUtilities.Requests;
using Shouldly;
using WorkTree.Application.UseCases.User.Create;
using WorkTree.Exceptions;

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


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Validation_ShouldHaveError_WhenNameIsEmpty(string name)
    {
        var request = RequestCreateUserJsonBuilder.Build();
        request.Name = name;

        var validator = new CreateUserValidator();

        var result = validator.Validate(request);

        result.Errors.ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_NAME_REQUIRED)),
        ]);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void Validation_ShouldHaveError_WhenEmailIsEmpty(string email)
    {
        var request = RequestCreateUserJsonBuilder.Build();
        request.Email = email;

        var validator = new CreateUserValidator();

        var result = validator.Validate(request);


        result.Errors.ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED))
        ]);
    }

    [Fact]
    public void Validation_ShouldHaveError_WhenPasswordIsEmpty()
    {
        var request = RequestCreateUserJsonBuilder.Build();
        request.Password = string.Empty;

        var validator = new CreateUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED)),
        ]);
    }
    
    [Theory]
    [InlineData("1234565")]
    public void Validation_ShouldHaveError_WhenPasswordDoesNotHaveMinimumLength(string password)
    {
        var request = RequestCreateUserJsonBuilder.Build();
        request.Password = password;

        var validator = new CreateUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_PASSWORD_MINIMUM_LENGTH)),
        ]);
    }

    [Fact]
    public void Validation_ShouldHaveError_WhenTenantIsEmpty()
    {
        var request = RequestCreateUserJsonBuilder.Build();
        request.TenantId = Guid.Empty;

        var validator = new CreateUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_TENANT_REQUIRED)),
        ]);
    }
}