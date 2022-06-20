
using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Comments.Commands;

public record UpdateCommentText(string Id, string Text) : IRequest
{
    public class Handler : IRequestHandler<UpdateCommentText>
    {
        private readonly IApplicationDbContext context;

        public Handler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateCommentText request, CancellationToken cancellationToken)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

            if (comment is null)
            {
                throw new Exception();
            }

            comment.UpdateText(request.Text);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}