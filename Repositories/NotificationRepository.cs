using JobPortal.Entities;
using JobPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext dbContext;

        public NotificationRepository( ) {
          this.dbContext=new ApplicationDbContext();
        }
        public async Task<int> CreateNotification(string userId, string msg)
        {
            Notification notification = new Notification();
            ApplicationUser user=await dbContext.Users.FirstOrDefaultAsync(x=>x.Id==userId);
            notification.User = user;
            notification.Message = msg;
            notification.ReceivedDate = DateTime.Now;
            dbContext.Notifications.Add(notification);
            dbContext.SaveChanges();
            return notification.Id;

        }

        public async Task<int> DeleteNotification(int id)
        {
            Notification notification=await dbContext.Notifications.FirstOrDefaultAsync(x=>x.Id==id);
             dbContext.Notifications.Remove(notification);
            await dbContext.SaveChangesAsync();
            return notification.Id;
        }

        public async Task<List<NotificationModel>> GetAllNotifications(string userId)
        {
            List<NotificationModel> userNotifications =(from n in  dbContext.Notifications where n.User.Id==userId 
                                      orderby n.ReceivedDate descending
                                      select new NotificationModel
                                      {
                                          NotificationId = n.Id,
                                          Message = n.Message,
                                          ReceivedDate=n.ReceivedDate
                                          
                                      }).ToList();
            return userNotifications;

        }
    }
}
