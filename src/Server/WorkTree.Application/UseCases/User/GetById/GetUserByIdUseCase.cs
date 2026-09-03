using WorkTree.Communication.Responses.Users;
using WorkTree.Domain.Repositories.User;
using WorkTree.Exceptions;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.Application.UseCases.User.GetById;

public class GetUserByIdUseCase : IGetUserByIdUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public GetUserByIdUseCase(IUserReadOnlyRepository userReadOnlyRepository)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task<ResponseUserJson> Execute(Guid userId)
    {
        var user = await _userReadOnlyRepository.FindByIdAsync(userId);

        if (user is null)
            throw new NotFoundErrorException(ResourceMessagesException.USER_NOT_FOUND);

        return new ResponseUserJson
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            TenantId = user.TenantId
        };
    }
}