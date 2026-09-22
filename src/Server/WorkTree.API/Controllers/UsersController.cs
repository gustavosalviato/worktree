using Microsoft.AspNetCore.Authorization;
using WorkTree.Communication.Responses;
using Microsoft.AspNetCore.Mvc;
using WorkTree.Application.UseCases.User.ChangePassword;
using WorkTree.Application.UseCases.User.Create;
using WorkTree.Application.UseCases.User.GetAll;
using WorkTree.Application.UseCases.User.GetById;
using WorkTree.Application.UseCases.User.Update;
using WorkTree.Communication.Requests.Users;
using WorkTree.Communication.Responses.Users;

namespace WorkTree.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : Controller
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Register([FromBody] RequestCreateUserJson requestCreate,
        [FromServices] ICreateUserUseCase useCase)
    {
        var response = await useCase.Execute(requestCreate);

        return Created(string.Empty, response);
    }

    [Authorize]
    [HttpPut("profile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] RequestUpdateUserJson request,
        [FromServices] IUpdateUserUseCase useCase)
    {
        await useCase.Execute(request);

        return NoContent();
    }
    
    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(ResponseUserJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] Guid userId, [FromServices] IGetUserByIdUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(List<ResponseUserJson>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllUsersUseCase useCase)
    {
        var users = await useCase.Execute();

        if (users.Count == 0)
            return NoContent();

        return Ok(users);
    }

    [Authorize]
    [HttpPut("password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] RequestChangePasswordJson request,
        [FromServices] IChangePasswordUseCase useCase)
    {
        await useCase.Execute(request);

        return NoContent();
    }
}