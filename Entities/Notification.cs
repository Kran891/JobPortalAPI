namespace JobPortal.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public DateTime ReceivedDate { get; set; }
        public ApplicationUser User { get; set; }
        public Company Company { get; set; }
        public string Message { get; set; }
    }
}
