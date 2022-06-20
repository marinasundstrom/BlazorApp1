
using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.Commands;

public record UpdateItemDescription(string Id, string Description) : IRequest
{
    public class Handler : IRequestHandler<UpdateItemDescription>
    {
        private readonly IApplicationDbContext context;

        public Handler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateItemDescription request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

            if (item is null)
            {
                throw new Exception();
            }

            item.SetDescription(request.Description);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}