namespace JobPortal.Models
{
    public class CompanyModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? OwnerId { get; set; }
        public int CompanyId { get; set; }
        public string? OwnerName { get; set; }
       public List<string>? CompanyLocations { get; set; }
    }
}
