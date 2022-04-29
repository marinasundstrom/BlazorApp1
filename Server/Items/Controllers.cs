using MediatR;
using Microsoft.AspNetCore.Mvc;
using BlazorApp1.Server.Items.Commands;
using BlazorApp1.Server.Items.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BlazorApp1.Server.Items.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly IMediator mediator;

    public ItemsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<PagedResult<ItemDto>>> GetItemsAsync(int page = 1, int pageSize = 0, string? createdBy = null, string? sortBy = null, SortDirection? sortDirection = null)
    {
        var result = await mediator.Send(new GetItemsQuery(page, pageSize, createdBy, sortBy, sortDirection));
        if(result is Result<PagedResult<ItemDto>, Exception>.Error(Exception Error)) 
        {
            return BadRequest();
        }
        return Ok((PagedResult<ItemDto>)result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ItemDto>> GetItemAsync(string id)
    {
        var item = await mediator.Send(new GetItemQuery(id));
        if(item is null) 
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost]
    public async Task CreateItem([FromBody] CreateItemDto value)
    {
        await mediator.Send(new CreateItemCommand(value.Name, value.Description));
    }

    [HttpPut("{id}/Description")]
    public async Task UpdateItemDescription(string id, [FromBody] UpdateItemDescriptionDto value)
    {
        await mediator.Send(new UpdateItemDescriptionCommand(id, value.Description));
    }

    [HttpDelete("{id}")]
    public async Task DeleteItem(string id)
    {
        await mediator.Send(new DeleteItemCommand(id));
    }
}

public record UpdateItemDescriptionDto(string Description);

public record CreateItemDto(string Name, string Description);
