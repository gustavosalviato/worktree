using Moq;
using WorkTree.Domain.Repositories.Tenant;

namespace CommonTestUtilities.Repositories;

public class TenantWriteOnlyRepositoryBuilder
{
    public static ITenantWriteOnlyRepository Build()
    {
        var mock = new Mock<ITenantWriteOnlyRepository>();

        return mock.Object;
    }
}