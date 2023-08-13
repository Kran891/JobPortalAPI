namespace JobPortal.Models
{
    public class StudentModel
    {
        public string StudentId { get; set; }
        public string Resume { get; set; }
        public string FullName { get; set; }
        public string? Address { get; set; }
       public List<string> studentskills { get; set; }
       public List<string> preferredLocations { get; set; }
       public IFormFile ResumeFile { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int AppliedId { get;  set; }
        public string InterViewDate { get;  set; }
        public string InterViewMode { get;  set; }
        public string InterViewLocation { get;  set; }
    }
}
