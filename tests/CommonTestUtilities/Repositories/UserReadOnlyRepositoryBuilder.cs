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

    public void FindManyByTenantIdAsync(Guid tenantId, List<User> users)
    {
        _mock.Setup(repository => repository.FindManyByTenantIdAsync(tenantId))
            .ReturnsAsync(users);
    }

    public void FindByIdAsync(User user)
    {
        _mock.Setup(repository => repository.FindByIdAsync(user.Id))
            .ReturnsAsync(user);
    }
    
    public IUserReadOnlyRepository Build() => _mock.Object;
}