
using BlazorApp1.Application.Common;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Utils;

namespace BlazorApp1.Application.Items.Commands;

public record UploadItemImage(string Id, Stream Stream, string FileName, string ContentType) : IRequest<UploadImageResult>
{
    public class UploadItemImageCommandHandler : IRequestHandler<UploadItemImage, UploadImageResult>
    {
        private readonly IApplicationDbContext context;
        private readonly IFileUploaderService _fileUploaderService;

        public UploadItemImageCommandHandler(IApplicationDbContext context, IFileUploaderService fileUploaderService)
        {
            this.context = context;
            this._fileUploaderService = fileUploaderService;
        }

        public async Task<UploadImageResult> Handle(UploadItemImage request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (item is null)
            {
                return UploadImageResult.Successful;
            }

            var guid = Guid.NewGuid();

            string imageId = $"image-{Guider.ToUrlFriendlyString(guid)}";

            await _fileUploaderService.UploadFileAsync(imageId, request.Stream, cancellationToken);

            item.ImageId = imageId;

            item.DomainEvents.Add(new ItemImageUploaded(item.Id, null!));

            await context.SaveChangesAsync(cancellationToken);

            return UploadImageResult.Successful;
        }
    }
}

public enum UploadImageResult
{
    Successful,
}