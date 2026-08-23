using Microsoft.AspNetCore.Mvc;
using WorkTree.API.UseCases.Tenants.Create;
using WorkTree.API.UseCases.Tenants.Delete;
using WorkTree.API.UseCases.Tenants.Update;
using WorkTree.Communication.Requests;
using WorkTree.Communication.Requests.Tenants;
using WorkTree.Communication.Responses;
using WorkTree.Communication.Responses.Tenants;

namespace WorkTree.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TenantsController : Controller
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseTenantJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status409Conflict)]
    public IActionResult Register([FromBody] RequestTenantJson request, [FromServices] CreateTenantUseCase useCase)
    {
        var response = useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPut]
    [Route("{tenantId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    public IActionResult Update([FromRoute] Guid tenantId, [FromBody] RequestUpdateTenantJson request,
        [FromServices] UpdateTenantUseCase useCase)
    {
        useCase.Execute(tenantId, request);

        return NoContent();
    }


    [HttpDelete]
    [Route("{tenantId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    public IActionResult Delete([FromRoute] Guid tenantId, [FromServices] DeleteTenantUseCase useCase)
    {
        useCase.Execute(tenantId);

        return Ok();
    }

    /*[HttpGet]
    [Route("{userId}")]
    [ProducesResponseType(typeof(ResponseUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    public IActionResult GetById([FromRoute] Guid userId, [FromServices] GetUserByIdUseCase useCase)
    {
        var response = useCase.Execute(userId);

        return Ok(response);
    }*/

    /*[HttpGet]
    [ProducesResponseType(typeof(List<ResponseUserJson>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status204NoContent)]
    public IActionResult GetAll([FromServices] GetAllUsersUseCase useCase)
    {
        var users = useCase.Execute();

        if (users.Count == 0)
            return NoContent();

        return Ok(users);
    }*/
}