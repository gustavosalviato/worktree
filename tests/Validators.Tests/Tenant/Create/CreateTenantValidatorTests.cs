using CommonTestUtilities.Requests;
using Shouldly;
using WorkTree.Application.UseCases.Tenant.Create;
using WorkTree.Exceptions;

namespace Validators.Tests.Tenant.Create;

public class CreateTenantValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RequestCreateTenantJsonBuilder.Build();

        var validator = new RequestTenantValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Validation_ShouldHaveError_WhenNameIsEmpty(string name)
    {
        var request = RequestCreateTenantJsonBuilder.Build();

        request.Name = name;

        var validator = new RequestTenantValidator();

        var result = validator.Validate(request);


        result.Errors.ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_NAME_REQUIRED)),
        ]);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Validation_ShouldHaveError_WhenEmailIsEmpty(string email)
    {
        var request = RequestCreateTenantJsonBuilder.Build();

        request.Email = email;

        var validator = new RequestTenantValidator();

        var result = validator.Validate(request);
        
        result.Errors.ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED)),
        ]);
    }
    
    [Theory]
    [InlineData("johndoe")]
    [InlineData("johndoe.com")]
    [InlineData("johndoegmail")]
    public void Validation_ShouldHaveError_WhenEmailIsInvalid(string email)
    {
        var request = RequestCreateTenantJsonBuilder.Build();

        request.Email = email;

        var validator = new RequestTenantValidator();

        var result = validator.Validate(request);
        
        result.Errors.ShouldSatisfy([
            e => e.Count.ShouldBe(1),
            e => e.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_EMAIL_INVALID)),
        ]);
    }
}