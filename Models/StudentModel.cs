namespace JobPortal.Models
{
    public class StudentModel
    {
        public string StudentId { get; set; }
        public string Resume { get; set; }
        public string FullName { get; set; }

       public List<string> studentskills { get; set; }
       public List<string> preferredLocations { get; set; }
    }
}
