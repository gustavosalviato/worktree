using Moq;
using WorkTree.Domain.Entities;
using WorkTree.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _mock;

    public UserReadOnlyRepositoryBuilder()
    {
        _mock = new Mock<IUserReadOnlyRepository>();
    }

    public void FindByEmailAsync(User user)
    {
        _mock.Setup(repository => repository.FindByEmailAsync(user.Email))
            .ReturnsAsync(user);
    }

    public IUserReadOnlyRepository Build() => _mock.Object;
}