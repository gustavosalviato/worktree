using Microsoft.AspNetCore.Mvc;
using WorkTree.API.UseCases.Session.Authenticate;
using WorkTree.API.UseCases.Session.RefreshToken;
using WorkTree.Communication.Requests.Auth;
using WorkTree.Communication.Responses;
using WorkTree.Communication.Responses.Auth;

namespace WorkTree.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(ResponseAuthenticateUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] RequestAuthenticateUserJson request,
        [FromServices] AuthenticateUserUseCase useCase)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }


    [HttpPost]
    [Route("refresh-token")]
    [ProducesResponseType(typeof(ResponseRefreshTokenJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RequestRefreshTokenJson request,
        [FromServices] RefreshTokenUseCase useCase)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }
}