using BlazorApp1.Application.Common;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Comments.Queries;

public record GetComment(string Id) : IRequest<CommentDto?>
{
    public class Handler : IRequestHandler<GetComment, CommentDto?>
    {
        private readonly IApplicationDbContext context;
        private readonly IUrlHelper _urlHelper;

        public Handler(IApplicationDbContext context, IUrlHelper urlHelper)
        {
            this.context = context;
            _urlHelper = urlHelper;
        }

        public async Task<CommentDto?> Handle(GetComment request, CancellationToken cancellationToken)
        {
            var item = await context.Comments
                .AsNoTracking()
                .AsSplitQuery()
                .IncludeAll()
                //.IgnoreQueryFilters()
                .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

            return item?.ToDto(_urlHelper);
        }
    }
}