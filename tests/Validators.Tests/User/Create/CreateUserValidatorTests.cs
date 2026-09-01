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


    [Fact]
    public void Validation_ShouldHaveError_WhenNameIsEmpty()
    {
        var request = RequestCreateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var validator = new CreateUserValidator();

        var result = validator.Validate(request);

        result.Errors.ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(error => error.ErrorMessage.Equals("Name could not be empty.")),
        ]);
    }

    [Fact]
    public void Validation_ShouldHaveError_WhenEmailIsEmpty()
    {
        var request = RequestCreateUserJsonBuilder.Build();
        request.Email = string.Empty;

        var validator = new CreateUserValidator();

        var result = validator.Validate(request);


        result.Errors.ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(error => error.ErrorMessage.Equals("Invalid email address."))
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
            e => e.Count.ShouldBe(2),
            e => e.ShouldContain(error => error.ErrorMessage.Equals("Password could not be empty.")),
            e => e.ShouldContain(error => error.ErrorMessage.Equals("Password must be at least 6 characters long.")),
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
            e => e.ShouldContain(error => error.ErrorMessage.Equals("Password could not be empty.")),
            e => e.ShouldContain(error => error.ErrorMessage.Equals("tenantId could not be empty.")),
        ]);
    }
}