namespace Core.DTOs.Responses;

public class FileDto
{
    public string FileId { get; set; }
    public string FileName { get; set; }
}

public class DownloadFileDto
{
    public DownloadFileDto(Stream data, string fileName)
    {
        Data = data;
        FileName = fileName;
    }
    public Stream Data { get; set; }
    public string FileName { get; set; }
}