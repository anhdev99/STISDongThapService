using Core.Events;
using MediatR;

namespace Infrastructure.Messaging.Consumers;

public class FileDeletedEventHandler() : INotificationHandler<FileDeletedEvent>
{
    public async Task Handle(FileDeletedEvent notification, CancellationToken cancellationToken)
    {
        if (File.Exists(notification.FilePath))
        {
            File.Delete(notification.FilePath);
        }
    }
}