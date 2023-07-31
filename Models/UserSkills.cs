namespace JobPortal.Models
{
    public class UserSkills
    {
        public int Id { get; set; }
        public Skills skill { get; set; }
        public ApplicationUser User { get; set; }
    }
}
