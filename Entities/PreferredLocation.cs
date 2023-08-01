namespace JobPortal.Entities
{
    public class PreferredLocation
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public ApplicationUser User { get; set; }
    }
}
