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

    public void FindByEmailAsync(string email)
    {
        _mock.Setup(repository => repository.FindByEmailAsync(email))
            .ReturnsAsync(new User("Gustavo", "gustavo@codifica.dev", Guid.NewGuid()));
    }

    public IUserReadOnlyRepository Build() => _mock.Object;
}