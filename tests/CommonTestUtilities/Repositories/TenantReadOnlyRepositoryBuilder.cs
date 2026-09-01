using Moq;
using WorkTree.Domain.Entities;
using WorkTree.Domain.Repositories.Tenant;

namespace CommonTestUtilities.Repositories;

public class TenantReadOnlyRepositoryBuilder
{
    private readonly Mock<ITenantReadOnlyRepository> _mock;

    public TenantReadOnlyRepositoryBuilder()
    {
        _mock = new Mock<ITenantReadOnlyRepository>();
    }

    public void FindByIdAsync(Guid id)
    {
        _mock.Setup(repository => repository.FindByIdAsync(id)).ReturnsAsync(new Tenant("John Doe", "johndoe@gmail"));
    }

    public ITenantReadOnlyRepository Build() => _mock.Object;
}