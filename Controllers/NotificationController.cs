using JobPortal.Models;
using JobPortal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationRepository notificationRepository;

        public NotificationController(INotificationRepository notificationRepository) {
            this.notificationRepository = notificationRepository;
        }
        [HttpGet]
        [Route("{userId}")]
       public async Task<IActionResult> GetAllNotifications(string userId)
        {
            try
            {
                var data = await notificationRepository.GetAllNotifications(userId);
                return Ok(new {data = data});
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                var data =await notificationRepository.DeleteNotification(id);
                return Ok(new {data = data});
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
