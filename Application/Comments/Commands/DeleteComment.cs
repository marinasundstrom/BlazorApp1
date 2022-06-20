
using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Comments.Commands;

public record DeleteComment(string ItemId, string CommentId) : IRequest
{
    public class Handler : IRequestHandler<DeleteComment>
    {
        private readonly IApplicationDbContext context;

        public Handler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteComment request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FirstOrDefaultAsync(item => item.Id == request.ItemId, cancellationToken);

            if (item is null)
            {
                throw new Exception();
            }

            var comment = await context.Comments.FirstOrDefaultAsync(comment => comment.Id == request.CommentId, cancellationToken);

            if (comment is null)
            {
                throw new Exception();
            }

            item.RemoveComment(comment);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}