namespace JobPortal.Entities
{
    public class StudentSkills
    {
        public int id { get; set; }
        public Skills skill { get; set; }
        public ApplicationUser user { get; set; }
        }
}
