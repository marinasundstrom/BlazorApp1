using System.Text.Json;

using BlazorApp1.Application.Common;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain;
using BlazorApp1.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Comments.Commands;

public record CreateComment(string ItemId, string Text) : IRequest<Result<CommentDto, Exception>>
{
    public class Handler : IRequestHandler<CreateComment, Result<CommentDto, Exception>>
    {
        private readonly IApplicationDbContext context;
        private readonly IUrlHelper _urlHelper;

        public Handler(IApplicationDbContext context, IUrlHelper urlHelper)
        {
            this.context = context;
            _urlHelper = urlHelper;
        }

        public async Task<Result<CommentDto, Exception>> Handle(CreateComment request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FirstOrDefaultAsync(item => item.Id == request.ItemId, cancellationToken);

            if (item is null)
            {
                throw new Exception();
            }

            var comment = new Comment(request.Text);

            item.AddComment(comment);

            await context.SaveChangesAsync(cancellationToken);

            comment = await context.Comments
                .AsNoTracking()
                .AsSplitQuery()
                .IncludeAll()
                .FirstAsync(i => i.Id == comment.Id, cancellationToken);

            //Console.WriteLine(JsonSerializer.Serialize(comment));

            return new Result<CommentDto, Exception>.Ok(comment.ToDto(_urlHelper));
        }
    }
}