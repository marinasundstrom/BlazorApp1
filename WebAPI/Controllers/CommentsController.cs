using BlazorApp1.Application;
using BlazorApp1.Application.Comments;
using BlazorApp1.Application.Comments.Commands;
using BlazorApp1.Application.Comments.Queries;
using BlazorApp1.Application.Items;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp1.WebAPI.Items.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly IMediator mediator;

    public CommentsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<PagedResult<CommentDto>>> GetComments(int page = 1, int pageSize = 10, string? itemId = null, string? createdBy = null, string? sortBy = null, SortDirection? sortDirection = null)
    {
        var result = await mediator.Send(new GetComments(page, pageSize, itemId, createdBy, sortBy, sortDirection));
        if (result is Result<PagedResult<CommentDto>, Exception>.Error(Exception Error))
        {
            return BadRequest();
        }
        return Ok((PagedResult<CommentDto>)result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CommentDto>> GetComment(string id, CancellationToken cancellationToken)
    {
        var item = await mediator.Send(new GetComment(id), cancellationToken);
        if (item is null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPut("{id}/Text")]
    public async Task UpdateText(string id, [FromBody] UpdateCommentTextDto value, CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateCommentText(id, value.Text), cancellationToken);
    }
}

public record UpdateCommentTextDto(string Text);

public record CreateCommentDto(string Text);