using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlazorApp1.Application.Items;
using BlazorApp1.Application.Items.Queries;
using BlazorApp1.Application;
using BlazorApp1.Application.Items.Commands;

namespace BlazorApp1.WebAPI.Items.Controllers;

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
    public async Task<ActionResult<PagedResult<ItemDto>>> GetItems(int page = 1, int pageSize = 0, string? createdBy = null, string? sortBy = null, SortDirection? sortDirection = null)
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
        await mediator.Send(new CreateItemCommand(value.Name, value.Description, value.StatusId));
    }

    [HttpPut("{id}/Status")]
    public async Task UpdateStatus(string id, [FromBody] UpdateItemStatusDto value)
    {
        await mediator.Send(new UpdateItemStatusCommand(id, value.StatusId));
    }


    [HttpPut("{id}/Description")]
    public async Task UpdateDescription(string id, [FromBody] UpdateItemDescriptionDto value)
    {
        await mediator.Send(new UpdateItemDescriptionCommand(id, value.Description));
    }

    [HttpDelete("{id}")]
    public async Task DeleteItem(string id)
    {
        await mediator.Send(new DeleteItemCommand(id));
    }
}

public record UpdateItemStatusDto(int StatusId);

public record UpdateItemDescriptionDto(string Description);

public record CreateItemDto(string Name, string Description, int StatusId);
