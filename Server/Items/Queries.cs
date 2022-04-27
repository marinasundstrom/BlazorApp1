using BlazorApp1.Server.Data;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Server.Items.Queries;

public record GetItemsQuery(int Page, int PageSize, string? CreatedBy, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<ItemsResult<ItemDto>>
{
    public class Handler : IRequestHandler<GetItemsQuery, ItemsResult<ItemDto>>
    {
        private readonly ApplicationDbContext context;

        public Handler(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<ItemsResult<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Items
                .AsNoTracking();

            if(request.CreatedBy is not null)
            {
                var createdBy = request.CreatedBy;

                query = query.Where(i => i.CreatedById == request.CreatedBy!);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                query = query.OrderBy(
                    request.SortBy, request.SortDirection.GetValueOrDefault());
            }

            query = query.Skip(request.Page * request.PageSize)
                .Take(request.PageSize).AsQueryable();

            var items = await query
                .AsSplitQuery()
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .Select(item => item.ToDto())
                .ToListAsync(cancellationToken);

            return new ItemsResult<ItemDto>(items, totalCount);
        }
    }
}

public record GetItemQuery(string Id) : IRequest<ItemDto>
{
    public class Handler : IRequestHandler<GetItemQuery, ItemDto>
    {
        private readonly ApplicationDbContext context;

        public Handler(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<ItemDto> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            var item = await context.Items
                .AsNoTracking()
                .AsSplitQuery()
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                //.Include(i => i.DeletedBy)
                //.IgnoreQueryFilters()
                .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

            if (item is null) {
                throw new Exception();
            }

            return item.ToDto();
        }
    }
}