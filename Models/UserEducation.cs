namespace JobPortal.Models
{
    public class UserEducation
    {
        public int Id { get; set; }
        public ApplicationUser applicationUser { get; set; }
        public Education Education { get; set; }
    }
}
