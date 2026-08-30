using Microsoft.AspNetCore.Mvc;
using WorkTree.Application.UseCases.Tenant.Create;
using WorkTree.Application.UseCases.Tenant.Delete;
using WorkTree.Application.UseCases.Tenant.GetById;
using WorkTree.Application.UseCases.Tenant.Update;
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
    public async Task<IActionResult> Register([FromBody] RequestTenantJson request,
        [FromServices] ICreateTenantUseCase useCase)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPut]
    [Route("{tenantId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid tenantId, [FromBody] RequestUpdateTenantJson request,
        [FromServices] IUpdateTenantUseCase useCase)
    {
        await useCase.Execute(tenantId, request);

        return NoContent();
    }


    [HttpDelete]
    [Route("{tenantId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid tenantId, [FromServices] IDeleteTenantUseCase useCase)
    {
        await useCase.Execute(tenantId);

        return Ok();
    }

    [HttpGet]
    [Route("{tenantId}")]
    [ProducesResponseType(typeof(ResponseTenantJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid tenantId, [FromServices] IGetTenantByIdUseCase useCase)
    {
        var response = await useCase.Execute(tenantId);

        return Ok(response);
    }
}