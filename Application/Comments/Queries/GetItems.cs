
using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Comments.Queries;

public record GetComments(int Page, int PageSize, string? ItemId, string? CreatedBy, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<Result<PagedResult<CommentDto>, Exception>>
{
    public class Handler : IRequestHandler<GetComments, Result<PagedResult<CommentDto>, Exception>>
    {
        private readonly IApplicationDbContext context;
        private readonly IUrlHelper _urlHelper;

        public Handler(IApplicationDbContext context, IUrlHelper urlHelper)
        {
            this.context = context;
            _urlHelper = urlHelper;
        }

        public async Task<Result<PagedResult<CommentDto>, Exception>> Handle(GetComments request, CancellationToken cancellationToken)
        {
            var query = context.Comments
                .Include(x => x.Item)
                .OrderByDescending(x => x.Created)
                .AsNoTracking();

            if (request.ItemId is not null)
            {
                query = query.Where(i => i.Item.Id == request.ItemId);
            }

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

            var comments = await query
                .AsSplitQuery()
                .IncludeAll()
                .ToListAsync(cancellationToken);

            var items2 = comments.Select(item => item.ToDto(_urlHelper));

            return new Result<PagedResult<CommentDto>, Exception>.Ok(new PagedResult<CommentDto>(items2, request.Page, totalCount));
        }
    }
}