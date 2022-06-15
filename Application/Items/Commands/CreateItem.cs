using BlazorApp1.Application.Common;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain;
using BlazorApp1.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.Commands;

public record CreateItem(string Name, string Description, int StatusId) : IRequest<Result<ItemDto, Exception>>
{
    public class Handler : IRequestHandler<CreateItem, Result<ItemDto, Exception>>
    {
        private readonly IApplicationDbContext context;
        private readonly IUrlHelper _urlHelper;

        public Handler(IApplicationDbContext context, IUrlHelper urlHelper)
        {
            this.context = context;
            _urlHelper = urlHelper;
        }

        public async Task<Result<ItemDto, Exception>> Handle(CreateItem request, CancellationToken cancellationToken)
        {
            var item = new Item(request.Name, request.Description, request.StatusId);

            context.Items.Add(item);

            await context.SaveChangesAsync(cancellationToken);

            item = await context.Items
                .AsNoTracking()
                .AsSplitQuery()
                .IncludeAll()
                .FirstAsync(i => i.Id == item.Id, cancellationToken);

            return new Result<ItemDto, Exception>.Ok(item.ToDto(_urlHelper));
        }
    }
}