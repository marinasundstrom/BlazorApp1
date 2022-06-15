using BlazorApp1.Application.Common;
using BlazorApp1.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.Commands;

public record DeleteItem(string Id) : IRequest
{
    public class Handler : IRequestHandler<DeleteItem>
    {
        private readonly IApplicationDbContext context;

        public Handler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteItem request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

            if (item is null)
            {
                throw new Exception();
            }

            context.Items.Remove(item);

            item.DomainEvents.Add(new ItemDeleted(item.Id));

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}