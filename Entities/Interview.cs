namespace JobPortal.Entities
{
    public class Interview
    {
        public int Id { get; set; }
        public DateTime InterViewDate { get; set; }
        public InterViewMode InterViewMode { get; set; }
        public string? InterViewLocation { get; set; }
        public AppliedJobs AppliedJob { get; set; }
    }
}
