

namespace JobPortal.Models
{
    public class JobModel
    {
        public int JobId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public double Salary { get; set; }
        public  List<string>? RequiredSkills { get; set; }
        public int NoOfApplicants { get;  set; }
        public List<string>? Locations { get;  set; }

        public DateTime InterViewDate { get; set; }
        public DateTime PostedDate { get; set; }

        public string? InterViewMode { get; set; }
    }
}
