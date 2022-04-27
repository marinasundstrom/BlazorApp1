using MediatR;
using Microsoft.EntityFrameworkCore;
using BlazorApp1.Server.Data;
using BlazorApp1.Server.Items.Events;
using BlazorApp1.Server.Models;

namespace BlazorApp1.Server.Items.Commands;

public record CreateItemCommand(string Name, string Description) : IRequest
{
    public class Handler : IRequestHandler<CreateItemCommand>
    {
        private readonly ApplicationDbContext context;
        private readonly IMediator mediator;

        public Handler(ApplicationDbContext context, IMediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var item = new Item() {
                Name = request.Name,
                Description = request.Description
            };

            context.Items.Add(item);

            await context.SaveChangesAsync(cancellationToken);

            await mediator.Publish(new ItemCreatedEvent(item.Id));

            return Unit.Value;
        }
    }
}

public record UpdateItemDescriptionCommand(string Id, string Description) : IRequest
{
    public class Handler : IRequestHandler<UpdateItemDescriptionCommand>
    {
        private readonly ApplicationDbContext context;
        private readonly IMediator mediator;

        public Handler(ApplicationDbContext context, IMediator mediator)
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

            await context.SaveChangesAsync(cancellationToken);

            await mediator.Publish(new ItemUpdatedEvent(item.Id));

            return Unit.Value;
        }
    }
}

public record DeleteItemCommand(string Id) : IRequest
{
    public class Handler : IRequestHandler<DeleteItemCommand>
    {
        private readonly ApplicationDbContext context;
        private readonly IMediator mediator;

        public Handler(ApplicationDbContext context, IMediator mediator)
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

            await context.SaveChangesAsync(cancellationToken);

            await mediator.Publish(new ItemDeletedEvent(item.Id));

            return Unit.Value;
        }
    }
}