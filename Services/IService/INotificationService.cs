using Unilever.CDExcellent.API.Models.Entities;

namespace Unilever.CDExcellent.API.Services.IService
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(int userId, string message);
        Task<IEnumerable<Notification>> GetNotificationsAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
    }
}
