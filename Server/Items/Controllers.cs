using MediatR;
using Microsoft.AspNetCore.Mvc;
using BlazorApp1.Server.Items.Commands;
using BlazorApp1.Server.Items.Queries;
using Microsoft.AspNetCore.Authorization;

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
    public async Task<ItemsResult<ItemDto>> GetItemsAsync(int page = 1, int pageSize = 0, string? createdBy = null, string? sortBy = null, SortDirection? sortDirection = null)
    {
        return await mediator.Send(new GetItemsQuery(page, pageSize, createdBy, sortBy, sortDirection));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ItemDto> GetItemAsync(string id)
    {
        return await mediator.Send(new GetItemQuery(id));
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
