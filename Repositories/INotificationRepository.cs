

using JobPortal.Models;

namespace JobPortal.Repositories
{
    public interface INotificationRepository
    {
        Task<int> CreateNotification(string userId,string msg);
        Task<List<NotificationModel>> GetAllNotifications(string userId);
        Task<int> DeleteNotification (int  id); 

    }
}
