using BlazorApp1.Application.Common;
using BlazorApp1.Application.Services;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.Queries;

public record GetItemsQuery(int Page, int PageSize, string? CreatedBy, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<Result<PagedResult<ItemDto>, Exception>>
{
    public class Handler : IRequestHandler<GetItemsQuery, Result<PagedResult<ItemDto>, Exception>>
    {
        private readonly IApplicationDbContext context;
        private readonly IUrlHelper _urlHelper;

        public Handler(IApplicationDbContext context, IUrlHelper urlHelper)
        {
            this.context = context;
            _urlHelper = urlHelper;
        }

        public async Task<Result<PagedResult<ItemDto>, Exception>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Items
                .OrderByDescending(x => x.Created)
                .AsNoTracking();

            if (request.CreatedBy is not null)
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

            query = query.Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize).AsQueryable();

            var items = await query
                .AsSplitQuery()
                .IncludeAll()
                .ToListAsync(cancellationToken);

            var items2 = items.Select(item => item.ToDto(_urlHelper));

            return new Result<PagedResult<ItemDto>, Exception>.Ok(new PagedResult<ItemDto>(items2, request.Page, totalCount));
        }
    }
}