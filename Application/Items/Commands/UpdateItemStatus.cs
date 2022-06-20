
using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.Commands;

public record UpdateItemStatus(string Id, int StatusId) : IRequest
{
    public class Handler : IRequestHandler<UpdateItemStatus>
    {
        private readonly IApplicationDbContext context;

        public Handler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateItemStatus request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

            if (item is null)
            {
                throw new Exception();
            }

            item.SetStatus(request.StatusId);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}