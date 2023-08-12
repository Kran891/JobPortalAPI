using System.Xml;

namespace JobPortal.Entities
{
    public class Jobs
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Company Company { get; set; }
        public double Salary { get; set; }
        
        public bool DeleteStatus { get; set; }=false;
        public DateTime PostedDate { get; set; }
    }
}
