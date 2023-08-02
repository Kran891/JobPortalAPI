

namespace JobPortal.Models
{
    public class JobModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public double Salary { get; set; }
        public  List<string> RequiredSkills { get; set; }
    }
}
