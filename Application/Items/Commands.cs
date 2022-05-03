﻿using BlazorApp1.Application.Common;
using BlazorApp1.Application.Items.Events;
using BlazorApp1.Domain;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.Commands;

public record CreateItemCommand(string Name, string Description) : IRequest<Result<Unit, Exception>>
{
    public class Handler : IRequestHandler<CreateItemCommand, Result<Unit, Exception>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMediator mediator;

        public Handler(IApplicationDbContext context, IMediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<Result<Unit, Exception>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var item = new Item() {
                Name = request.Name,
                Description = request.Description
            };

            context.Items.Add(item);

            item.DomainEvents.Add(new ItemCreatedEvent(item.Id));

            await context.SaveChangesAsync(cancellationToken);

            return new Result<Unit, Exception>.Ok(Unit.Value);
        }
    }
}

public record UpdateItemDescriptionCommand(string Id, string Description) : IRequest
{
    public class Handler : IRequestHandler<UpdateItemDescriptionCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly IMediator mediator;

        public Handler(IApplicationDbContext context, IMediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateItemDescriptionCommand request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

            if (item is null) {
                throw new Exception();
            }

            item.Description = request.Description;

            item.DomainEvents.Add(new ItemUpdatedEvent(item.Id));

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

public record DeleteItemCommand(string Id) : IRequest
{
    public class Handler : IRequestHandler<DeleteItemCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly IMediator mediator;

        public Handler(IApplicationDbContext context, IMediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

            if (item is null) {
                throw new Exception();
            }

            context.Items.Remove(item);
            
            item.DomainEvents.Add(new ItemDeletedEvent(item.Id));

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}