namespace JobPortal.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ApplicationUser Owner { get; set; }
        public virtual List<CompanyLocation> Locations { get; set; }
        public bool Status { get; set; }
        public bool DeleteStatus { get; set; }=false;

    }
}
