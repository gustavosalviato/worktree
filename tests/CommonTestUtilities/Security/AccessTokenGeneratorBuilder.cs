using Bogus;
using Moq;
using WorkTree.Domain.Entities;
using WorkTree.Domain.Security.Tokens;

namespace CommonTestUtilities.Security;

public class AccessTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();

        var fakeToken = new Faker().Random.String2(32);
        
        mock.Setup(generator => generator.Generate(It.IsAny<User>())).Returns(fakeToken);

        return mock.Object;
    }
}