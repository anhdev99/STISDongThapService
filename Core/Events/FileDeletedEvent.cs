using Shared;

namespace Core.Events;

public class FileDeletedEvent : BaseEvent
{
    public string FilePath { get; set; }

    public FileDeletedEvent(string filePath)
    {
        FilePath = filePath;
    }
}