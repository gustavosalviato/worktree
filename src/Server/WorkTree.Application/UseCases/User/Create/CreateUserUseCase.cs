using Mapster;
using WorkTree.Communication.Requests.Users;
using WorkTree.Communication.Responses.Users;
using WorkTree.Domain.Repositories;
using WorkTree.Domain.Repositories.User;
using WorkTree.Domain.Security.PasswordHashing;
using WorkTree.Exceptions.ExceptionsBase;

namespace WorkTree.Application.UseCases.User.Create;

public class CreateUserUseCase : ICreateUserUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserUseCase(IPasswordHasher passwordHasher, IUserWriteOnlyRepository userWriteOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository, IUnitOfWork unitOfWork)
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseUserJson> Execute(RequestUserJson request)
    {
        await ValidateAndThrowOnFailure(request);

        var user = request.Adapt<Domain.Entities.User>();

        var passwordHashed = _passwordHasher.HashPassword(request.Password);

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

    private async Task ValidateAndThrowOnFailure(RequestUserJson request)
    {
        var validator = new RequestUserValidator();

        var result = await validator.ValidateAsync(request);

        var exists = await _userReadOnlyRepository.FindByEmailAsync(request.Email);

        if (exists is not null)
            throw new ConflictErrorException("User with this email already exists.");

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}