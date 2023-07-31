namespace JobPortal.Models
{
    public class Interview
    {
        public int Id { get; set; }
        public DateTime InterViewDate { get; set; }
        public InterViewMode InterViewMode { get; set; }
        public Jobs job { get; set; }
    }
}
