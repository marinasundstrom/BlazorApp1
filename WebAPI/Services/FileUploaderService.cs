using Azure.Storage.Blobs;

using BlazorApp1.Application.Services;

namespace BlazorApp1.WebAPI.Services;

public class FileUploaderService : IFileUploaderService
{
    private readonly BlobServiceClient _blobServiceClient;

    public FileUploaderService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task DeleteFileAsync(string id, CancellationToken cancellationToken = default)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient("images");

        var response = await blobContainerClient.DeleteBlobIfExistsAsync(id);
    }

    public async Task UploadFileAsync(string id, Stream stream, CancellationToken cancellationToken = default)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient("images");

        var response = await blobContainerClient.UploadBlobAsync(id, stream, cancellationToken);
    }
}