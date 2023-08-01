namespace JobPortal.Entities
{
    public class AppliedJobs
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public Jobs Job { get; set; }
    }
}
