using BlazorApp1.Application;
using BlazorApp1.Application.Comments.Commands;
using BlazorApp1.Application.Items;
using BlazorApp1.Application.Items.Commands;
using BlazorApp1.Application.Items.Queries;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BlazorApp1.Application.Comments;

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
    public async Task<ActionResult<PagedResult<ItemDto>>> GetItems(int page = 1, int pageSize = 10, [FromQuery] int[]? statusId = null, string? createdBy = null, string? sortBy = null, SortDirection? sortDirection = null)
    {
        var result = await mediator.Send(new GetItems(page, pageSize, statusId, createdBy, sortBy, sortDirection));
        if (result is Result<PagedResult<ItemDto>, Exception>.Error(Exception Error))
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
    public async Task<ActionResult<ItemDto>> GetItemAsync(string id, CancellationToken cancellationToken)
    {
        var item = await mediator.Send(new GetItem(id), cancellationToken);
        if (item is null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost]
    public async Task<ItemDto> CreateItem([FromBody] CreateItemDto value, CancellationToken cancellationToken)
    {
        return await mediator.Send(new CreateItem(value.Name, value.Description, value.StatusId), cancellationToken);
    }

    [HttpPost("{id}/UploadImage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UploadImage([FromRoute] string id, IFormFile file, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new UploadItemImage(id, file.OpenReadStream(), file.Name, file.ContentType), cancellationToken);
    }

    [HttpPut("{id}/Status")]
    public async Task UpdateStatus(string id, [FromBody] UpdateItemStatusDto value, CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateItemStatus(id, value.StatusId), cancellationToken);
    }


    [HttpPut("{id}/Description")]
    public async Task UpdateDescription(string id, [FromBody] UpdateItemDescriptionDto value, CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateItemDescription(id, value.Description), cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteItem(string id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteItem(id), cancellationToken);
    }

    [HttpPost("{id}/Comments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CommentDto>> AddComment(string id, [FromBody] CreateCommentDto value, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateComment(id, value.Text), cancellationToken);
        if (result is Result<CommentDto, Exception>.Error(Exception Error))
        {
            return BadRequest();
        }
        return Ok((CommentDto)result);
    }


    [HttpDelete("{id}/Comments/{commentId}")]
    public async Task DeleteComment(string id, string commentId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteComment(id, commentId), cancellationToken);
    }
}

public record UpdateItemStatusDto(int StatusId);

public record UpdateItemDescriptionDto(string Description);

public record CreateItemDto(string Name, string Description, int StatusId);