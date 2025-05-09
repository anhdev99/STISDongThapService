namespace Core.DTOs;

public class ExceptionDetail
{
    public int Code { get; set; }
    public string type { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
}