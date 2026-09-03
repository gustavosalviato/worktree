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

    public void FindByIdAsync(Tenant tenant)
    {
        _mock.Setup(repository => repository.FindByIdAsync(tenant.Id)).ReturnsAsync(tenant);
    }

    public ITenantReadOnlyRepository Build() => _mock.Object;
}