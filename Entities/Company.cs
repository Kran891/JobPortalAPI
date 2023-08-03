namespace JobPortal.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ApplicationUser Owner { get; set; }
        public virtual List<CompanyLocation> Locations { get; set; }

    }
}
