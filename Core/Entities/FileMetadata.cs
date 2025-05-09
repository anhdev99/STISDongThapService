using Shared;

namespace Core.Entities;

public class FileMetadata : BaseAuditableEntity
{
    public string FileId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public string? Module { get; set; }
}