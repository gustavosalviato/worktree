using WorkTree.Communication.Responses;
using Microsoft.AspNetCore.Mvc;
using WorkTree.API.UseCases.Users.Create;
using WorkTree.API.UseCases.Users.Delete;
using WorkTree.API.UseCases.Users.GetAll;
using WorkTree.API.UseCases.Users.GetById;
using WorkTree.API.UseCases.Users.Update;
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
    public IActionResult Register([FromBody] RequestUserJson request, [FromServices] CreateUserUseCase useCase)
    {
        var response = useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPut]
    [Route("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    public IActionResult Update([FromRoute] Guid userId, [FromBody] RequestUpdateUserJson request,
        [FromServices] UpdateUserUseCase useCase)
    {
        useCase.Execute(userId, request);

        return NoContent();
    }


    [HttpDelete]
    [Route("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    public IActionResult Delete([FromRoute] Guid userId, [FromServices] DeleteUserUseCase useCase)
    {
        useCase.Execute(userId);

        return Ok();
    }

    [HttpGet]
    [Route("{userId}")]
    [ProducesResponseType(typeof(ResponseUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    public IActionResult GetById([FromRoute] Guid userId, [FromServices] GetUserByIdUseCase useCase)
    {
        var response = useCase.Execute(userId);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ResponseUserJson>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status204NoContent)]
    public IActionResult GetAll([FromServices] GetAllUsersUseCase useCase)
    {
        var users = useCase.Execute();

        if (users.Count == 0)
            return NoContent();

        return Ok(users);
    }
}