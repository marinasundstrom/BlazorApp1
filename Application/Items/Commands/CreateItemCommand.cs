using BlazorApp1.Application.Common;
using BlazorApp1.Domain.Events;
using BlazorApp1.Domain;

using MediatR;

namespace BlazorApp1.Application.Items.Commands;

public record CreateItemCommand(string Name, string Description) : IRequest<Result<Unit, Exception>>
{
    public class Handler : IRequestHandler<CreateItemCommand, Result<Unit, Exception>>
    {
        private readonly IApplicationDbContext context;

        public Handler(IApplicationDbContext context)
        {
            this.context = context;
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
