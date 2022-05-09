using BlazorApp1.Application.Common;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.Queries;

public record GetItemQuery(string Id) : IRequest<ItemDto?>
{
    public class Handler : IRequestHandler<GetItemQuery, ItemDto?>
    {
        private readonly IApplicationDbContext context;
        private readonly IUrlHelper _urlHelper;

        public Handler(IApplicationDbContext context, IUrlHelper urlHelper)
        {
            this.context = context;
            _urlHelper = urlHelper;
        }

        public async Task<ItemDto?> Handle(GetItemQuery request, CancellationToken cancellationToken)
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