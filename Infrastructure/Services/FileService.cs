using AutoMapper;
using Core.DTOs.Responses;
using Core.Entities;
using Core.Events;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class FileService(
    IHttpContextAccessor httpContextAccessor,
    IWebHostEnvironment _hostingEnvironment,
    ILogger<BaseService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IWebHostEnvironment _env)
    : BaseService(httpContextAccessor, logger, dbContext, unitOfWork, mapper), IFileService
{
    public async Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var rootDirectory = "UploadedFiles";
        var fileId = Guid.NewGuid().ToString();
        var fileName = $"{fileId}_{Path.GetFileName(file.FileName)}";

        var folderPath = Path.Combine(_env.ContentRootPath, rootDirectory);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var fullPath = Path.Combine(rootDirectory, fileName);

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        var metadata = new FileMetadata
        {
            FileId = fileId,
            FileName = file.FileName,
            FilePath = fullPath,
            ContentType = file.ContentType,
            Size = file.Length,
        };

        await _unitOfWork.Repository<FileMetadata>().AddAsync(metadata);
        await _unitOfWork.Save(cancellationToken);

        return fileId;
    }

    public async Task<(Stream fileStream, string contentType, string fileName)> DownloadFileAsync(string fileId,
        CancellationToken cancellationToken)
    {
        var file = await _unitOfWork.Repository<FileMetadata>().Entities
            .Where(x => x.FileId == fileId && x.IsDeleted != true).FirstOrDefaultAsync(cancellationToken);

        if (file == null)
            throw new FileNotFoundException();
        string mediaPath = Path.Combine(_env.ContentRootPath, file.FilePath);
        if (!File.Exists(mediaPath))
        {
            throw new Exception("File not found");
        }

        var stream = new FileStream(mediaPath, FileMode.Open, FileAccess.Read);
        return (stream, file.ContentType, file.FileName);
    }

    public async Task DeleteFileAsync(string fileId, CancellationToken cancellationToken)
    {
        var file = await _unitOfWork.Repository<FileMetadata>().Entities
            .Where(x => x.FileId == fileId && x.IsDeleted != true).FirstOrDefaultAsync(cancellationToken);

        if (file == null || file.IsDeleted)
            throw new FileNotFoundException();
        string mediaPath = Path.Combine(_env.ContentRootPath, file.FilePath);
        if (!File.Exists(mediaPath))
        {
            throw new Exception("File not found");
        }

        file.IsDeleted = true;
        file.AddDomainEvent(new FileDeletedEvent(mediaPath));
        await _unitOfWork.Save(cancellationToken);
    }

    public async Task<DownloadFileDto> DownloadByFileIdAsync(string fileId, CancellationToken cancellationToken)
    {
        var entity = _unitOfWork.Repository<FileMetadata>().Entities.Where(x => x.FileId == fileId && x.IsDeleted != true)
            .FirstOrDefault();
        if(entity == null)
            throw new FileNotFoundException("Không tìm thấy tệp tin!");
                
        string mediaPath =  Path.Combine(_hostingEnvironment.ContentRootPath, entity.FilePath ?? string.Empty);
        if (!File.Exists(mediaPath))
        {
            throw new Exception("File not found");
        }

        
        using (FileStream fileStream = new FileStream(mediaPath, FileMode.Open, FileAccess.Read))
        {
            MemoryStream memoryStream = new MemoryStream();
            fileStream.CopyTo(memoryStream);
            memoryStream.Position = 0;
            return new DownloadFileDto(memoryStream, entity.FileName); 
        }
    }
}