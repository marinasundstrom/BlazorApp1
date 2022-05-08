using BlazorApp1.Application.Common;
using BlazorApp1.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.Commands;

public record UpdateItemDescriptionCommand(string Id, string Description) : IRequest
{
    public class Handler : IRequestHandler<UpdateItemDescriptionCommand>
    {
        private readonly IApplicationDbContext context;

        public Handler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateItemDescriptionCommand request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

            if (item is null)
            {
                throw new Exception();
            }

            item.Description = request.Description;

            item.DomainEvents.Add(new ItemUpdatedEvent(item.Id));

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}