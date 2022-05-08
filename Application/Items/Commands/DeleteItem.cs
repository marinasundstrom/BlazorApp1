using BlazorApp1.Application.Common;
using BlazorApp1.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.Commands;

public record DeleteItemCommand(string Id) : IRequest
{
    public class Handler : IRequestHandler<DeleteItemCommand>
    {
        private readonly IApplicationDbContext context;

        public Handler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

            if (item is null)
            {
                throw new Exception();
            }

            context.Items.Remove(item);

            item.DomainEvents.Add(new ItemDeletedEvent(item.Id));

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}