namespace JobPortal.Entities
{
    public class CompanyLocation
    {
        public int Id { get; set; }
        public Company Company { get; set; }
        public Location Location { get; set; }
    }
}
