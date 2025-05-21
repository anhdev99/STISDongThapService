using Core.DTOs.Responses;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces;

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default);
    Task<(Stream fileStream, string contentType, string fileName)> DownloadFileAsync(string id, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string fileId, CancellationToken cancellationToken);
    Task<DownloadFileDto> DownloadByFileIdAsync(string fileId, CancellationToken cancellationToken);
}