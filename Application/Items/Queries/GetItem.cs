
using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.Queries;

public record GetItem(string Id) : IRequest<ItemDto?>
{
    public class Handler : IRequestHandler<GetItem, ItemDto?>
    {
        private readonly IApplicationDbContext context;
        private readonly IUrlHelper _urlHelper;

        public Handler(IApplicationDbContext context, IUrlHelper urlHelper)
        {
            this.context = context;
            _urlHelper = urlHelper;
        }

        public async Task<ItemDto?> Handle(GetItem request, CancellationToken cancellationToken)
        {
            var item = await context.Items
                .AsNoTracking()
                .AsSplitQuery()
                .IncludeAll()
                //.IgnoreQueryFilters()
                .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

            return item?.ToDto(_urlHelper);
        }
    }
}