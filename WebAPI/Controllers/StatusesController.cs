﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlazorApp1.Application.Items;
using BlazorApp1.Application.Items.Queries;
using BlazorApp1.Application;

namespace BlazorApp1.WebAPI.Items.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class StatusesController : ControllerBase
{
    private readonly IMediator mediator;

    public StatusesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<StatusDto>>> GetStatuses()
    {
        return Ok(await mediator.Send(new GetStatusesQuery()));
    }

}
