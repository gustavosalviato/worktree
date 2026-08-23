using Microsoft.AspNetCore.Mvc;
using WorkTree.API.UseCases.Session.Authenticate;
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
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    public IActionResult Login([FromBody] RequestAuthenticateUserJson request,
        [FromServices] AuthenticaUseUseCase useCase)
    {
        var response = useCase.Execute(request);

        return Created(string.Empty, response);
    }
}