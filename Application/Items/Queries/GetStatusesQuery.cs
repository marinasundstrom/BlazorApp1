using BlazorApp1.Application.Common;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.Queries;

public record GetStatusesQuery() : IRequest<IEnumerable<StatusDto>>
{
    public class Handler : IRequestHandler<GetStatusesQuery, IEnumerable<StatusDto>>
    {
        private readonly IApplicationDbContext context;

        public Handler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<StatusDto>> Handle(GetStatusesQuery request, CancellationToken cancellationToken)
        {
            return await context.Statuses
                .Select(s => s.ToDto())
                .ToArrayAsync();
        }
    }
}