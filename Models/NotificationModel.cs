namespace JobPortal.Models
{
    public class NotificationModel
    {
        public string userId {  get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public int NotificationId { get; set; }
        public DateTime ReceivedDate { get;  set; }
    }
}
