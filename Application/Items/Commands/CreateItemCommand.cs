using BlazorApp1.Application.Common;
using BlazorApp1.Domain;
using BlazorApp1.Domain.Events;

using MediatR;

namespace BlazorApp1.Application.Items.Commands;

public record CreateItemCommand(string Name, string Description) : IRequest<Result<ItemDto, Exception>>
{
    public class Handler : IRequestHandler<CreateItemCommand, Result<ItemDto, Exception>>
    {
        private readonly IApplicationDbContext context;

        public Handler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<ItemDto, Exception>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var item = new Item()
            {
                Name = request.Name,
                Description = request.Description
            };

            context.Items.Add(item);

            item.DomainEvents.Add(new ItemCreatedEvent(item.Id));

            await context.SaveChangesAsync(cancellationToken);

            return new Result<ItemDto, Exception>.Ok(item.ToDto());
        }
    }
}
