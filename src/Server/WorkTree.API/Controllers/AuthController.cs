using Microsoft.AspNetCore.Mvc;
using WorkTree.Application.UseCases.Authentication.Authenticate.WithEmailAndPassword;
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
    [ProducesResponseType(typeof(ResponseAuthenticateUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] RequestAuthenticateJson request,
        [FromServices] IAuthenticateWithEmailAndPasswordUserUseCase useCase)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}