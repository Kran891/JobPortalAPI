namespace JobPortal.Models
{
    public class InterViewModel
    {
        public int Id { get; set; }
        public int AppliedId { get; set; }
        public string? InterViewMode { get; set; }
        public DateTime InterViewDate { get; set; }
        public string? JobName { get; set; }
        public string? CompanyName { get; set; }
        public string? InterViewLocation { get; set; }
    }
}
