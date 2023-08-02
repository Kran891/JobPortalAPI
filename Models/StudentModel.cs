namespace JobPortal.Models
{
    public class StudentModel
    {
        public string Id { get; set; }
        public string Resume { get; set; }
       

       public List<string> studentskills { get; set; }
       public List<string> preferredLocations { get; set; }
    }
}
