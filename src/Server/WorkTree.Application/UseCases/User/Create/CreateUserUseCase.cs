using Mapster;
using WorkTree.Communication.Requests.Users;
using WorkTree.Communication.Responses.Users;
using WorkTree.Domain.Repositories;
using WorkTree.Domain.Repositories.Tenant;
using WorkTree.Domain.Repositories.User;
using WorkTree.Domain.Security.PasswordHashing;
using WorkTree.Exceptions;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.Application.UseCases.User.Create;

public class CreateUserUseCase : ICreateUserUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly ITenantReadOnlyRepository _tenantReadOnlyRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserUseCase
    (
        IPasswordHasher passwordHasher,
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        ITenantReadOnlyRepository readOnlyRepository,
        IUnitOfWork unitOfWork
    )
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _tenantReadOnlyRepository = readOnlyRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseUserJson> Execute(RequestCreateUserJson requestCreate)
    {
        await ValidateAndThrowOnFailures(requestCreate);

        var user = requestCreate.Adapt<Domain.Entities.User>();

        var passwordHashed = _passwordHasher.HashPassword(requestCreate.Password);

        user.ChangePassword(passwordHashed);

        await _userWriteOnlyRepository.AddAsync(user);

        await _unitOfWork.CommitAsync();

        return new ResponseUserJson
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            TenantId = user.TenantId
        };
    }

    private async Task ValidateAndThrowOnFailures(RequestCreateUserJson requestCreate)
    {
        var validator = new CreateUserValidator();

        var result = await validator.ValidateAsync(requestCreate);

        var tenantExists = await _tenantReadOnlyRepository.FindByIdAsync(requestCreate.TenantId);

        if (tenantExists is null)
        {
            throw new NotFoundErrorException(ResourceMessagesException.ORGANIZATION_NOT_FOUND);
        }

        var exists = await _userReadOnlyRepository.FindByEmailAsync(requestCreate.Email);

        if (exists is not null)
            throw new ConflictErrorException(ResourceMessagesException.USER_WITH_EMAIL_ALREADY_EXISTS);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}