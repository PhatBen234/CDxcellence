using Unilever.CDExcellent.API.Models.Entities;

namespace Unilever.CDExcellent.API.Services.IService
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(int senderId, int receiverId, string title, string message);
        Task<IEnumerable<Notification>> GetNotificationsAsync(int userId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId);
        Task<IEnumerable<Notification>> SearchNotificationsAsync(int userId, string keyword);
        Task<Notification?> GetNotificationByIdAsync(int notificationId);
        Task MarkAsReadAsync(int notificationId);
    }
}
